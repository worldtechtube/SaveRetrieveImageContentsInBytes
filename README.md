Client side implementation: https://github.com/worldtechtube/SaveAndRetrieveImageContentInAngular

DB related:
Wallpaper table creation:


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[wallpaper](
	[id] [int] NOT NULL,
	[wallpaperContent] [varbinary](max) NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


Stored Procedure for inserting the image content record:

CREATE PROCEDURE dbo.Wallpaper_insert  
(    
  @id INT   
, @wallpaperContent VARBINARY(MAX)    
)    
AS    
BEGIN    
  SET NOCOUNT ON;    
    
  INSERT INTO dbo.wallpaper    
  ( id    
  , wallpaperContent    
  )    
  VALUES    
  ( @id    
  , @wallpaperContent    
  );    
END;

Query to retrieve:
SELECT [id]
      ,[wallpaperContent]
  FROM [StudentDB].[dbo].[wallpaper]
