# TODO items

Any task or thing that needs to get done can be added here.

## TODO Queue
* Production is showing developer messages
* Implement post deletion by administrators.
* Implement post flagging.
* Fix it so 300 errors properly redirect to login-register page.
* Understand why re-log is required after every deployment
* delivery and ask submit pages are copy-pasted and should be refactored.
* Investigate race conditions seen when generating content with simultaneous calls.

## Tech Backlog

* Back button should return user to previous position on page.
* Update ValidationProblemDetails to use camelCase properties in the errors.
* Generate and add thumbnail links to images. https://learn.microsoft.com/en-us/dotnet/api/system.drawing.image.getthumbnailimage?view=dotnet-plat-ext-7.0
* Add proper log reporting
* Add TraceId to requests and responses
* Setup email server
* Add domain event handling when a user registers
* Add ability to delete Asks, Deliveries, Comments, Tags, Images
* API is using forms, so add CSRF protecting
* Make nginx r. proxy work with azure storage redirect. It seems to fail because it can't handle http -> https redirects.

## Feature Backlog

* Add Daily, Weekly, Monthly challenges
* Rewards for challenges
* Add front page "Hot" query that ensures deliveries have priority.
* Tag based search
* User and Account page
* Email user when upvotes, comments or deliveries are posted.
