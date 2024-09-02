# Camply Receiver

The purpose of this code is to work with .Net Minimal API's to gain experience and to create a middleware between Camply, a super cool camping app for site searches and availability checking written in Python, and a Bot Project written in .Net.

This code will act as middle ware for a bot assistant I am writing that will look like the following when I am done. THe goal is to give me some experience using the minimal API pattern to implement an API Middleware that is representative of what a client or an organization would typically do for something tactical like integrating with a piece of legacy code or an existing system to provide users with a desired UX. In my case this is a bot, that calls an API which will in turn execute the Camply codebase (the integration with the existing system idea from above) which then calls the API Middleware which produces the result for the bot to consume and present back to the user.


### Architectural Diagram (9/1/2024)
![Current Architectural Plan](/Docs/camply-bot-integration.png)


### Goal Project Architecture (8/30/2024)

1. **Microsoft Bot Assistant**: 
   - **Bot Interface**: This is where the bot interacts with users, receiving commands and sending responses.
   - **Azure Queue (Bot Queue)**: The bot subscribes to this queue to get responses from the API middleware. The bot listens for messages in this queue to receive search results.

2. **API Middleware**:
   - **Webhook Endpoint**: The webhook receives search requests from the bot, processed by the Python script.
   - **Python Script (Camply)**: 
     - This script contains constructors for search actions, such as searching for campsite availability or listing campsites at a specific campground.
     - Executes the search actions using the Jutfin Camply library.
   - **Search Results**: Once the search is executed, the results are prepared to be sent back to the bot.
   - **Add Result to Azure Queue**: The search results are added to the Azure Queue, which the Microsoft Bot Assistant subscribes to.

### Status: POC In Progress
- Create Python Script to run Camply (as the eventual Azure Function that will receive HTTP requests from the bot)
- Create simple API project to figure out a path for a full implementation of the Camply bot integration results parser
- Sketch out the goal architecture