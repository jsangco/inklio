# TODO

Any task or thing that needs to get done can be added here.

## Priority
- [ ] Fix Odata warnings
- [ ] Design badges system
- [ ] Add captcha to user account creation.
- [ ] Implement post flagging.
- [ ] (I think the lifetimescope changes fixed this; retest is needed) Investigate race conditions seen when generating content with simultaneous calls.
- [ ] Add UI for tags and tagging
- [ ] User and Account page
- [ ] Setup email server
- [ ] Investigate entity association debug warnings (they don't cause problem atm)

## Tech Backlog

- [ ] Delivery and ask submit pages are copy-pasted and should be refactored.
- [ ] Back button should return user to previous position on page.
- [ ] Update ValidationProblemDetails to use camelCase properties in the errors.
- [ ] Generate and add thumbnail links to images. https://learn.microsoft.com/en-us/dotnet/api/system.drawing.image.getthumbnailimage?view=dotnet-plat-ext-7.0
- [ ] Add proper log reporting
- [ ] Add TraceId to requests and responses
- [ ] Add domain event handling when a user registers
- [ ] API is using multi-part forms. Is CSRF protection necessary?
- [ ] Run perf and load test to understand limitations of service.
- [ ] Make nginx r. proxy work with azure storage redirect. It seems to fail because it can't handle http -> https redirects.

## Feature Backlog

- [ ] Tag based search
- [ ] Email user when upvotes, comments or deliveries are posted.
- [ ] User social media links

## Completed
- [x] Ask, Delivery, Comment, Challenge CRUD API 
- [x] Lock/Unlock Ask API