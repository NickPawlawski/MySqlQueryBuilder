# MySqlQueryBuilder

This is a c# library built to allow developers to quickly build queries and execute them with transactional support and feature rich reporting.

This library was inspired by Laravel's Eloquent ORM and tried to share some of its ideas for the building blocks of the code. 

# Using the library

Each of the four queries , insert, update, delete, and select, has its own syntax. Each will be displayed below. 

### Initializing the library

To initialize the library, call the function SetServerString within the DatabaseConnection class and pass it the parameters

> server      
> userid      
> password    
> database    

Once this method is called it does not need to be called again. You can now use the rest of the software. 

#### The Parent

The parent builder is the class responsible for managing a single transaction. Create a parent using the parentbuilder constuctor. 

`
ParentBuilder _pb = new ParentBuilder();
`

#### The Child

The child builder class is responsible for a single query. Each child gets loaded into the parentbuilder class. When the parent runs 
each of the childs queries are ran in the sequence they were loaded into the parent. Here is an example of creating a new child. 
Details of this process will be shown later.

`
new ChildBuilder("categories", dict, ParentBuilder.Statements.Select)
`


