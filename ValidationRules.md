# General rules for a valid request body
There are a few rules that must be followed to ensure the request body is accepted as valid when using `POST` and `PUT` methods. The following list summarizes the general rules for a valid request body:

* serviceId within ServiceDTO should not be specified even though it is included within the request body example.

* All Id fields within ServiceDTO must be greater than 0 as they represent identity columns within the database. 

* Unique fields can not be null or ignored. When adding a new record to any table, ensure the value passed is unique and does not exist within the database. Otherwise, the existing record will be referenced instead. For more information about unique fields see the list of unique keys per for each table at the bottom of this document.

* Adding a new service requires either adding a new record in another table to reference it or referencing an existing record.

* If you wish to add a new record in another table to reference it, ensure the table’s corresponding Id field is null in ServiceDTO. Not passing the field altogether should be fine as it will assign null to the field implicitly. For example, when adding a new service with a new program, add the relevant information about the new program as such:
  ```javascript
  {
    // ...
    "programId": null,
    "programDTO": {
      // information about new program ...
    }
    // ...
  }
  ```
  * This includes all the existing DTOs present within the example request body.

* if you wish to reference an existing record in another table, ensure the table’s corresponding DTO in the request body is null. For example, when adding a new service with a reference to an existing program, add a reference to the record using Id field in ServiceDTO as such:
 
  ```javascript
  {
    // ...
    "programId": 1,
    "programDTO": null,
    // ...
  }
  ```
  * This includes all the existing DTOs present within the example request body. 


## List of Unique Keys Per Table:
* Service: 
  * primary key(s): __serviceId__
  * Unique key(s): `None`
* Program:
  * Primary key(s): __programId__
  * Unique key(s): `None`
* Department:
  * Primary key(s): __departmentId__
  * Unique key(s): __departmentCode__
* Division:
  * Primary key(s): __divisionId__
  * Unique key(s): __divisionCode__
* Contact:
  * primary key(s): __contactId__
  * Unique key(s): `None`
* Community:
  * Primary key(s): __communityId__
  * Unique key(s): __communityName__
* Language:
  * Primary key(s): __languageId__
  * Unique key(s): __languageName__
* Location:
  * Primary key(s): __locationId__
  * Unique key(s): __locationName__
* LocationType:
  * Primary key(s): __locationTypeId__
  * Unique key(s): __locationTypeName__


