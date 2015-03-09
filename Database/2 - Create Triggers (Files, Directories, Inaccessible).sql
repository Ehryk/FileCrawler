
USE FileCrawler

GO

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
			InsertDate = SYSDATETIME()
	from	Directories
	join	Inserted on  Directories.ID = Inserted.ID
END

GO
				
-- Creating update trigger for Log4Net
	create trigger [cnx_custom].[TR_CNX_LOGS_Upd_Audit] ON [cnx_custom].[CNX_LOGS]
				for update
				as
				begin
					if update(InsUser) or update (InsDate)
					begin
						update	[CNX_LOGS]
						set 		InsUser = Deleted.InsUser, 
									InsDate = Deleted.InsDate
						from		[CNX_LOGS]
						join		Inserted on   [CNX_LOGS].[LogId] = Inserted.[LogId]
						join 		Deleted on  [CNX_LOGS].[LogId] = Deleted.[LogId]
						where		Deleted.InsUser is not null 
						and			Deleted.InsDate is not null
					end		

					update	[CNX_LOGS] 
					set		UpdUser = suser_sname(),
							UpdDate = SYSDATETIME()
					from	[CNX_LOGS]
					join	Inserted on  [CNX_LOGS].[LogId] = Inserted.[LogId]

				end


GO


