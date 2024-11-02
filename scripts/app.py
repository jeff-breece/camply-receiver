from flask import Flask, request, jsonify
import subprocess
import requests

app = Flask(__name__)

WEBHOOK_URL = "http://localhost:5000/api/webhook/camply"

@app.route('/camply/execute', methods=['POST'])
def execute_camply():
    data = request.get_json()
    
    # Extract parameters
    command_type = data.get("command_type")
    options = data.get("options", {})
    provider = options.get("provider", "OhioStateParks")
    campground_id = options.get("campground")
    start_date = options.get("start-date")
    end_date = options.get("end-date")

    print("Received JSON data:", data)
    print("Command Type:", command_type)
    print("Options:", options)
    print("Campground ID:", campground_id)
    print("Start Date:", start_date)
    print("End Date:", end_date)
    print("Provider:", provider)

    # Build the command
    if command_type == "list-campsites":
        command = ["camply", "list-campsites", "--campground", str(campground_id), "--provider", provider]
    elif command_type == "campsites":
        if not (start_date and end_date and campground_id):
            return jsonify({"error": "Missing required parameters for 'campsites' command"}), 400
        command = [
            "camply", "campsites",
            "--campground", str(campground_id),
            "--start-date", start_date,
            "--end-date", end_date,
            "--provider", provider
        ]
    else:
        return jsonify({"error": "Invalid command type"}), 400

    # Execute command
    try:
        result = subprocess.run(command, capture_output=True, text=True, check=True)
        output = {
            "command": command_type,
            "stdout": result.stdout,
            "stderr": result.stderr,
            "returncode": result.returncode
        }
        
        # Send the result to the webhook
        webhook_response = requests.post(WEBHOOK_URL, json=output)
        if webhook_response.status_code == 200:
            return jsonify({"message": "Successfully sent to webhook", "data": output}), 200
        else:
            return jsonify({
                "error": "Failed to send to webhook",
                "webhook_status_code": webhook_response.status_code,
                "webhook_response": webhook_response.text
            }), 500
    except subprocess.CalledProcessError as e:
        return jsonify({
            "error": "Camply command execution failed",
            "stdout": e.stdout,
            "stderr": e.stderr
        }), 500

if __name__ == '__main__':
    app.run(port=8000)