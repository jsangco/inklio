# Controllers

# Notes

Configuring optimal OData quries when the Domain objects and DTOs are separated can be tricky. The following sample code is one way to enable OData on such queries

```
[HttpGet]
// [EnableQuery(PageSize=2)] // Disabled. When an OData query is applied manually, this attribute will cause the OData query to execute twice
public IQueryable<Inklio.Api.Models.Ask>? Get(ODataQueryOptions options)
{
    var settings =  new ODataQuerySettings();
    settings.PageSize = 2;

     // Manually apply the odata query to the domain object. This doesn't work if the DTO object
     // has properties that the domain does not. It's also possible to use AutoMapper to project
     // from the domain to the DTO and then apply the OData query. This is probably safer, but
     // there may be a performance hit; not sure.
    var odataQueryResult = options.ApplyTo(DomainAsks.AsQueryable(), settings);

    var domainQueryResult = odataQueryResult as result as IQueryable<Inklio.Api.Models.Ask>;
    dtoResults = domainQueryResult.ProjectTo<Inklio.Api.Models.Ask>(this.mapperConfiguration); // map using automapper

    return dtoResults;
}
```