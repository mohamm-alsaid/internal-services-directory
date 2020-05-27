# Adjusting the API for Schema Changes

- The Model

All that has to be changed for the relevant model (the table that you modified), is that you need to simply add a public member variable for the new column, or adjust an existing variable to accomodate a name change. For example, adding
`public Boolean Active { get; set; }` to the `Service.cs` model. Nullables will have a `?` after the type, if you modified the schema to make a column not nullable from being nullable, remove the questionmark. Or add a question mark if you made a column nullable.

- The DTO

The change for the DTO is exactly the same as the model. Just add a member variable for the new column, or adjust an existing variable.

- DTO Extension

No change is necessary for the Nullable quality here. However, the DTOExtension file for the given model and DTO must be adjusted so that the two copy methods from either direction, the CopyFrom methods in particular, to make sure that the new column is being transfered in the transformation between Model and DTO. For example, for the active boolean, add the following line to `CopyFromServiceV1DTO()` and `CopyFromService`.

```
    to.Active = from.Active;
```

- `InternalServicesDirectoryV1Context.cs`

The change for this file is the most complicated. Inside of the definition of the object, there's a method called `OnModelCreating`. This method is called when the database context talks to the database, and is how EntityFramework Core translates the database information into our model. Inside of this method are a set of lambda functions for each "entity", in a call called `modelBuilder.Entity<Service>` for the Service entity for example. If you added a column, you need to add code to the body of the lambda function to describe the schema.

Generally, it's only a few lines per column. For example, when I added the active column to the Service table, I had to add the following block to the modelBuilder for Service:
```cs
entity.Property(e => e.Active)
                    .HasColumnName("active")
                    .HasColumnType("bit");
```

I specify that the entity has a property, `Active`, I tell EFCore what that property's column name is in the database, and I specify the SQL Server datatype. For a string/varchar, you'll have to specify its maximum length.

No change is necessary for the Nullable quality here.
