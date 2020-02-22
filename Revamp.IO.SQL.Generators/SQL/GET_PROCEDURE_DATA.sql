--DECLARE  @I_PROC_NAME varchar(MAX) = 'SP_U_' + 'OBJ_PROP_OPT_SETS' 
--DECLARE @I_SCHEMA varchar(MAX) = 'CSA'
select * from (select @I_SCHEMA [schema], '' datatype, 'V_PROCEDURE_NAME' paramname, @I_PROC_NAME proc_name, 'public string V_PROCEDURE_NAME {get; set;} = "' + UPPER(@I_PROC_NAME) + '";' [c# model syntax], '' parameter_id, '' [name], 0 max_length, 0 [precision], 0 scale
union 
select @I_SCHEMA [schema], '' datatype, 'V_ATTEMPTED_SQL' paramname, @I_PROC_NAME proc_name, 'public string V_ATTEMPTED_SQL {get; set;}' [c# model syntax], '' parameter_id, '' [name], 0 max_length, 0 [precision], 0 scale
union
Select sh.name [schema], t.name datatype, p.name paramname, so.name proc_name, 
	'public ' + CASE 
	WHEN t.name = 'int' then 'int' 
	WHEN t.name = 'bigint' then 'long?' 
	WHEN t.name = 'date' then 'DateTime?' 
	WHEN t.name = 'varchar' then 'string'
	WHEN t.name = 'char' then 'char?'
	WHEN t.name = 'varbinary' then 'byte[]'
	WHEN t.name = 'bit' then 'bool'
	WHEN t.name = 'datetime' then 'DateTime?'
	when t.name = 'uniqueidentifier' then 'Guid?'
	else t.name  END + ' ' + SUBSTRING(p.name, 2, LEN(p.name)) + ' { get; set; }' [c# model syntax],
	P.parameter_id,
	SO.[name], 
	P.max_length,
	P.[precision],
	P.scale
FROM sys.objects as SO
INNER JOIN sys.parameters as P
	ON SO.object_id = p.object_id
inner join sys.schemas sh on (sh.schema_id = so.schema_id)
inner join sys.types t on (t.system_type_id = p.system_type_id)
WHERE SO.object_id in (select object_id from sys.objects where type in ('P'))
and sh.name = @I_SCHEMA and so.name = @I_PROC_NAME) a
ORDER BY  name, parameter_id