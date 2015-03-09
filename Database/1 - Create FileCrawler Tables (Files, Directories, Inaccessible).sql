
USE FileCrawler
GO

CREATE TABLE Files_Loading
(
	ID int primary key not null identity(1,1),
	[CrawlPath] nvarchar(1000) not null,

	[Path] nvarchar(1000) not null,
	
	[Root] nvarchar(100) null,
	IsNetwork bit not null default 0,
	IsLocal bit not null default 0,

	Directory nvarchar(1000) null,
	ParentName nvarchar(1000) null,
	Name nvarchar(1000) not null,
	Extension nvarchar(1000) null,
	[ReadOnly] bit not null default 0,
	Attributes int null,
	
	IsContainer bit not null default 0,
	IsContained bit not null default 0,
	ContainerPath nvarchar(1000) null,
	ContainerName nvarchar(1000) null,
	ContainedDirectory nvarchar(1000) null,
	ContainedPath nvarchar(1000) null,

	Size bigint null,
	KB decimal null,
	MB decimal null,
	GB decimal null,

	CreateTime datetime null,
	LastAccessTime datetime null,
	LastWriteTime datetime null,

	InsertUser nvarchar(1000) not null,
	InsertDate datetime2 not null,
	UpdateUser nvarchar(1000) null,
	UpdateDate datetime2 null
)

GO

CREATE TABLE Files_Deleted
(
	ID int primary key not null,
	[CrawlPath] nvarchar(1000) not null,

	[Path] nvarchar(1000) not null,
	
	[Root] nvarchar(100) null,
	IsNetwork bit not null default 0,
	IsLocal bit not null default 0,

	Directory nvarchar(1000) null,
	ParentName nvarchar(1000) null,
	Name nvarchar(1000) not null,
	Extension nvarchar(1000) null,
	[ReadOnly] bit not null default 0,
	Attributes int null,
	
	IsContainer bit not null default 0,
	IsContained bit not null default 0,
	ContainerPath nvarchar(1000) null,
	ContainerName nvarchar(1000) null,
	ContainedDirectory nvarchar(1000) null,
	ContainedPath nvarchar(1000) null,

	Size bigint null,
	KB decimal null,
	MB decimal null,
	GB decimal null,

	CreateTime datetime null,
	LastAccessTime datetime null,
	LastWriteTime datetime null,
	
	InsertUser nvarchar(1000) not null,
	InsertDate datetime2 not null,
	UpdateUser nvarchar(1000) null,
	UpdateDate datetime2 null,
	DeleteUser nvarchar(1000) not null,
	DeleteDate datetime2 not null
)

GO

CREATE TABLE Files
(
	ID int primary key not null identity(1,1),
	[CrawlPath] nvarchar(1000) not null,

	[Path] nvarchar(1000) not null,
	
	[Root] nvarchar(100) null,
	IsNetwork bit not null default 0,
	IsLocal bit not null default 0,

	Directory nvarchar(1000) null,
	ParentName nvarchar(1000) null,
	Name nvarchar(1000) not null,
	Extension nvarchar(1000) null,
	[ReadOnly] bit not null default 0,
	Attributes int null,
	
	IsContainer bit not null default 0,
	IsContained bit not null default 0,
	ContainerPath nvarchar(1000) null,
	ContainerName nvarchar(1000) null,
	ContainedDirectory nvarchar(1000) null,
	ContainedPath nvarchar(1000) null,

	Size bigint null,
	KB decimal null,
	MB decimal null,
	GB decimal null,

	CreateTime datetime null,
	LastAccessTime datetime null,
	LastWriteTime datetime null,

	InsertUser nvarchar(1000) not null,
	InsertDate datetime2 not null,
	UpdateUser nvarchar(1000) null,
	UpdateDate datetime2 null
)

GO

CREATE TABLE Directories_Deleted
(
	ID int primary key not null,
	[CrawlPath] nvarchar(1000) not null,

	[Path] nvarchar(1000) not null,
	
	[Root] nvarchar(100) null,
	IsNetwork bit not null default 0,
	IsLocal bit not null default 0,

	Name nvarchar(1000) not null,
	ParentName nvarchar(1000) null,
	[ReadOnly] bit not null default 0,
	Attributes int null,

	FileCount int null,
	TotalSize bigint null,

	CreateTime datetime null,
	LastAccessTime datetime null,
	LastWriteTime datetime null,
	
	InsertUser nvarchar(1000) not null,
	InsertDate datetime2 not null,
	UpdateUser nvarchar(1000) null,
	UpdateDate datetime2 null,
	DeleteUser nvarchar(1000) not null,
	DeleteDate datetime2 not null
)

GO

CREATE TABLE Directories
(
	ID int primary key not null identity(1,1),
	[CrawlPath] nvarchar(1000) not null,

	[Path] nvarchar(1000) not null,
	
	[Root] nvarchar(100) null,
	IsNetwork bit not null default 0,
	IsLocal bit not null default 0,

	Name nvarchar(1000) not null,
	ParentName nvarchar(1000) null,
	[ReadOnly] bit not null default 0,
	Attributes int null,

	FileCount int null,
	TotalSize bigint null,

	CreateTime datetime null,
	LastAccessTime datetime null,
	LastWriteTime datetime null,

	InsertUser nvarchar(1000) not null,
	InsertDate datetime2 not null,
	UpdateUser nvarchar(1000) null,
	UpdateDate datetime2 null
)

CREATE TABLE Inaccessible
(
	ID int primary key not null identity(1,1),
	[CrawlPath] nvarchar(1000) not null,

	[Path] nvarchar(1000) not null,
	
	[Root] nvarchar(100) null,
	IsNetwork bit not null default 0,
	IsLocal bit not null default 0,

	IsFile bit not null default 0,
	IsFolder bit not null default 0,

	InsertUser nvarchar(1000) not null,
	InsertDate datetime2 not null,
	UpdateUser nvarchar(1000) null,
	UpdateDate datetime2 null
)

GO
