USE [Inventory]

go

/****** Object:  StoredProcedure [dbo].[AddNewCategory]    Script Date: 4/6/2023 10:17:27 AM ******/
SET ansi_nulls ON

go

SET quoted_identifier ON

go

ALTER PROCEDURE [dbo].[Addnewcategory] @CategoryName VARCHAR(50),
                                       @CategoryID   INT = 0
AS
  BEGIN
      SET nocount ON;

      IF EXISTS (SELECT 1
                 FROM   [dbo].[Categories]
                 WHERE  [Categoryname] = @CategoryName)
        BEGIN
			THROW 50000, 'Category name already exists!', 1;
        END

      BEGIN try;
          BEGIN TRANSACTION;

          INSERT INTO [dbo].[Categories]
                      (Categoryname)
          VALUES      (@CategoryName);

          COMMIT TRANSACTION;

          SELECT @CategoryID = Scope_identity();

          EXEC Getcategorybyid
            @CategoryID = @CategoryID;
      END try

      BEGIN catch;
          IF ( @@TRANCOUNT > 0 )
            BEGIN;
                ROLLBACK TRANSACTION;
            END;

          PRINT 'Error ocurred in ' + Error_procedure() + ' '
                + Error_message();

          RETURN -1;
      END catch;

      RETURN 0;
  END; 