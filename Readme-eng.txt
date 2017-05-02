To use the web-apllication you have to:

1. Execute the OwnersAndPetsDataCreation.sql script at databases server
If error message
"Cannot insert the value NULL into column 'PetId', table 'OwnersAndPetsDB.dbo.OwnerPets'; column does not allow nulls. INSERT fails"
have been displayed, select the last 2 lines of script text and execute them separately

2. Run the OwnersAndPets.sln solution file

3. In Solution Explorer window open the Web.config file, add to the connectionStrings tag this line
<add name="default" connectionString="data source=[PC_name/resourse_address]\[Server_instance_name];Initial Catalog=OwnersAndPetsDB;Trusted_Connection=True;" />
where 'data source' is databases server address

4. Launch the web application