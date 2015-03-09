
USE FileCrawler
GO

/*
DROP TRIGGER trg_Directories_Insert
DROP TRIGGER trg_Directories_Update
DROP TRIGGER trg_Directories_Delete

DROP TRIGGER trg_Files_Insert
DROP TRIGGER trg_Files_Update
DROP TRIGGER trg_Files_Delete
*/

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER trg_Directories_Insert ON Directories
FOR INSERT
AS
BEGIN

	update	Directories
	set		InsertUser = suser_sname(),
			InsertDate = sysdatetime()
	from	Directories
	join	Inserted on Directories.ID = Inserted.ID

END

GO

CREATE TRIGGER trg_Directories_Update ON Directories
FOR UPDATE
AS
BEGIN

	if update(InsertUser) or update (InsertDate)
	begin
		update		Directories
		set 		InsertUser = Deleted.InsertUser, 
					InsertDate = Deleted.InsertDate
		from		Directories
		join		Inserted on Directories.ID = Inserted.ID
		join 		Deleted  on Directories.ID = Deleted.ID
		where		Deleted.InsertUser is not null 
		and			Deleted.InsertDate is not null
	end		

	update	Directories 
	set		UpdateUser = suser_sname(),
			UpdateDate = sysdatetime()
	from	Directories
	join	Inserted on Directories.ID = Inserted.ID

END

GO

CREATE TRIGGER trg_Directories_Delete ON Directories
FOR DELETE
AS
BEGIN

	insert into Directories_Deleted
	(
		ID,
		[CrawlPath],

		[Path],
	
		[Root],
		IsNetwork,
		IsLocal,

		Name,
		ParentName,
		[ReadOnly],
		Attributes,

		FileCount,
		TotalSize,

		CreateTime,
		LastAccessTime,
		LastWriteTime,

		InsertUser,
		InsertDate,
		UpdateUser,
		UpdateDate,
		DeleteUser,
		DeleteDate
	)
	select 
		ID,
		[CrawlPath],

		[Path],
	
		[Root],
		IsNetwork,
		IsLocal,

		Name,
		ParentName,
		[ReadOnly],
		Attributes,

		FileCount,
		TotalSize,

		CreateTime,
		LastAccessTime,
		LastWriteTime,

		InsertUser,
		InsertDate,
		UpdateUser,
		UpdateDate,
		suser_sname(),
		sysdatetime()
	from Deleted

END

GO

CREATE TRIGGER trg_Files_Insert ON Files
FOR INSERT
AS
BEGIN

	update	Files
	set		InsertUser = suser_sname(),
			InsertDate = sysdatetime()
	from	Files
	join	Inserted on Files.ID = Files.ID

END

GO

CREATE TRIGGER trg_Files_Update ON Files
FOR UPDATE
AS
BEGIN

	if update(InsertUser) or update (InsertDate)
	begin
		update		Files
		set 		InsertUser = Deleted.InsertUser, 
					InsertDate = Deleted.InsertDate
		from		Files
		join		Inserted on Files.ID = Inserted.ID
		join 		Deleted  on Files.ID = Deleted.ID
		where		Deleted.InsertUser is not null 
		and			Deleted.InsertDate is not null
	end		

	update	Files 
	set		UpdateUser = suser_sname(),
			UpdateDate = sysdatetime()
	from	Files
	join	Inserted on Files.ID = Inserted.ID

END

GO

CREATE TRIGGER trg_Files_Delete ON Files
FOR DELETE
AS
BEGIN

	insert into Files_Deleted
	(
		ID,
		[CrawlPath],

		[Path],
	
		[Root],
		IsNetwork,
		IsLocal,

		Directory,
		ParentName,
		Name,
		Extension,
		[ReadOnly],
		Attributes,
	
		IsContainer,
		IsContained,
		ContainerPath,
		ContainerName,
		ContainedDirectory,
		ContainedPath,

		Size,
		KB,
		MB,
		GB,

		CreateTime,
		LastAccessTime,
		LastWriteTime,

		InsertUser,
		InsertDate,
		UpdateUser,
		UpdateDate,
		DeleteUser,
		DeleteDate
	)
	select 
		ID,
		[CrawlPath],

		[Path],
	
		[Root],
		IsNetwork,
		IsLocal,
		
		Directory,
		ParentName,
		Name,
		Extension,
		[ReadOnly],
		Attributes,
	
		IsContainer,
		IsContained,
		ContainerPath,
		ContainerName,
		ContainedDirectory,
		ContainedPath,

		Size,
		KB,
		MB,
		GB,

		CreateTime,
		LastAccessTime,
		LastWriteTime,

		InsertUser,
		InsertDate,
		UpdateUser,
		UpdateDate,
		suser_sname(),
		sysdatetime()
	from Deleted

END

GO
