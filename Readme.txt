��� ������������� ���-���������� ����������:

1. ��������� �� ������� ��� ������ ������ OwnersAndPetsDataCreation.sql
���� ��� ���������� ������� ����� �������� ���������
Cannot insert the value NULL into column 'PetId', table 'OwnersAndPetsDB.dbo.OwnerPets'; column does not allow nulls. INSERT fails,
�� �������� ��������� 2 ������ ������ ������� � ��������� �� ��������

2. ��������� ���-������ OwnersAndPets.sln

3. � ���� Solution Explorer ������� ���� Web.config, �������� � ������ connectionStrings ������
<add name="default" connectionString="data source=[���_����������]\[���_����������_�������];Initial Catalog=OwnersAndPetsDB;Trusted_Connection=True;" />
�������� data source - ��� ����� ������� ��� ������.

4. ��������� ���-����������