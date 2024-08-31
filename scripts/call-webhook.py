import subprocess
import requests

# Search parameters
campground_id = "554"
campsite = "23738"
start_date = "2024-08-31"
end_date = "2024-09-01"
provider = "OhioStateParks"
webhook_url = "http://localhost:5000/api/webhook/camply"  # Use HTTP and port 5000

# Command Codes
# ToDo: THis script file will become an Azure Function with an HTTP Trigger
# Commmands to include the various Camply Search Types
# "campsite" - search specific site availablity at a park
# "list-campsites" - list all sites at a park
# "campground" - search sites available at a park


# ToDo: Break this script out into one that reads from an input file or Web API to get a users search parameters from the BOT
# Site Listing for a given park
# Run the camply CLI command to list campsites
#command = [
#    "camply", "list-campsites",
#    "--campground", str(campground_id),
#    "--provider", provider
#]

# Specific Site Search
# camply campsites   --provider OhioStateParks   --campground 554   --campsite 23738   --start-date 2024-08-31   --end-date 2024-09-01
command = [
    "camply", "campground",
    "--campground", str(campground_id),
    "--campsite", str(campsite),
    "--start-date", start_date,
    "--end-date", end_date,
    "--provider", provider
]

print("Constructed command:", command)

# Code for the Park Site(s) listing call
result = subprocess.run(command, capture_output=True, text=True)

# Print the command output for debugging
print("Command output:", result.stdout)
print("Command error (if any):", result.stderr)

# Extract relevant information
output = {
    "command": "campsite",
    "stdout": result.stdout,  # The standard output of the command
    "stderr": result.stderr,  # The standard error of the command
    "returncode": result.returncode  # The return code of the command
}

# Send the result to the webhook
response = requests.post(webhook_url, json=output)

# Check the response from the webhook
if response.status_code == 200:
    print("Successfully sent to webhook")
else:
    print(f"Failed to send to webhook, status code: {response.status_code}")

# Code for the Park Site(s) listing call
'''
try:
    result = subprocess.run(command, capture_output=True, text=True, check=True)
    output = result.stdout
    if result.returncode != 0:
        print(f"Camply command failed with error:\n{result.stderr}")
    else:
        # Process the plain text output
        campsites = []
        for line in output.split('\n'):
            if 'Campsite' in line:
                campsites.append(line.strip())

        # Notify the webhook if there are available sites
        if campsites:
            for site in campsites:
                notification = {
                    'Site': site,
                    'Availability': 'Unknown',  # You can modify this if you have specific date information
                    'Date': 'Unknown'  # You can modify this if you have specific date information
                }
                response = requests.post(
                    webhook_url, json=notification
                )
                if response.status_code == 200:
                    print(f"Notification sent for site: {notification['Site']}")
                else:
                    print(f"Failed to send notification for site: {notification['Site']}")
except subprocess.CalledProcessError as e:
    print(f"An error occurred while running camply: {e}")
    print(f"Standard Output:\n{e.stdout}")
    print(f"Standard Error:\n{e.stderr}")
'''