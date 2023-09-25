# TODO items

Any task or thing that needs to get done can be added here.

* Create production nginx.conf that replaces the the local when running in build pipeline
* Understand why re-log is required after every deployment
* Investigate file size limits. asp.net issue? ngnix issue?
* Add upvote, add flag, to AskCard
* Update readme test query commands
* delivery and ask submit pages are copy-pasted and should be refactored.

## Tech Backlog

* Update ValidationProblemDetails to use camelCase properties in the errors.
* Add proper log reporting
* Add TraceId to requests and responses
* Setup email server
* Add domain event handling when a user registers
* Add ability to delete Asks, Deliveries, Comments, Tags, Images
* Investigate race conditions seen when generating content with simultaneous calls.
* API is using forms, so add CSRF protecting
* Make ngnix r. proxy work with azure storage redirect. It seems to fail because it can't handle http -> https redirects.

## Feature Backlog

* Add Daily, Weekly, Monthly challenges
* Add front page "Hot" query that ensures deliveries have priority.
* Tag based search
* User and Account page
