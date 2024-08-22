import subprocess
import requests

# Define your search parameters
campground_id = 554
provider = "OhioStateParks"
webhook_url = "http://localhost:5000/api/webhook/camply"  # Use HTTP and port 5000

# Run the camply CLI command to list campsites
command = [
    "camply", "list-campsites",
    "--campground", str(campground_id),
    "--provider", provider
]

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
