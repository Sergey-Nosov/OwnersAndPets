Для использования веб-приложения необходимо:

1. Запустить на сервере баз данных скрипт OwnersAndPetsDataCreation.sql
Если при выполнении скрипта будет выведено сообщение
Cannot insert the value NULL into column 'PetId', table 'OwnersAndPetsDB.dbo.OwnerPets'; column does not allow nulls. INSERT fails,
то выделить последние 2 строки текста скрипта и выполнить их отдельно

2. Запустить веб-проект OwnersAndPets.sln

3. В окне Solution Explorer открыть файл Web.config, добавить в раздел connectionStrings строку
<add name="default" connectionString="data source=[Имя_компьютера]\[Имя_экземпляра_сервера];Initial Catalog=OwnersAndPetsDB;Trusted_Connection=True;" />
Параметр data source - это адрес сервера баз данных.

4. Запустить веб-приложение