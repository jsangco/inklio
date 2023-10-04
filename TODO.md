# TODO items

Any task or thing that needs to get done can be added here.

## TODO Queue
* User and Account page
* Implement post flagging.
* Investigate race conditions seen when generating content with simultaneous calls.
* Add captcha to user account creation.
* Add UI for tags and tagging

## Tech Backlog

* Setup email server
* Delivery and ask submit pages are copy-pasted and should be refactored.
* Back button should return user to previous position on page.
* Update ValidationProblemDetails to use camelCase properties in the errors.
* Generate and add thumbnail links to images. https://learn.microsoft.com/en-us/dotnet/api/system.drawing.image.getthumbnailimage?view=dotnet-plat-ext-7.0
* Add proper log reporting
* Add TraceId to requests and responses
* Add domain event handling when a user registers
* API is using forms, so add CSRF protection
* Run perf and load test to understand limitations service.
* Make nginx r. proxy work with azure storage redirect. It seems to fail because it can't handle http -> https redirects.

## Feature Backlog

* Add Daily, Weekly, Monthly challenges
* Rewards for challenges
* Tag based search
* Email user when upvotes, comments or deliveries are posted.
