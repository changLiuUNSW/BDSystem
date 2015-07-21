USE [IMS_Test]
GO
/****** Object:  StoredProcedure [dbo].[GetLostjobIndex]    Script Date: 21/07/2015 10:32:32 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[GetLostjobIndex]
	@StartRecord INT,
	@Length INT,
	@OrderBy NVARCHAR(50),
	@OrderType NVARCHAR(50),
	@JobNumber NVARCHAR(50),
	@Location NVARCHAR(50),
	@FinalCleanStart DATETIME,
	@FinalCleanEnd DATETIME,
	@AddedDateStart DATETIME,
	@AddedDateEnd DATETIME,
	@ConfirmDateStart DATETIME,
	@ConfirmDateEnd DATETIME,
	@LostJobConfirmed NVARCHAR(10) = 'All',
	@DA NVARCHAR(10) = 'All',	
	@Compliance NVARCHAR(10) = 'All',
	@CE NVARCHAR(10) = 'All',
	@Contracts NVARCHAR(10) = 'All',
	@CC NVARCHAR(10) = 'All',
	@Accounts NVARCHAR(10) = 'All',
	@AR NVARCHAR(10) = 'All'
AS
BEGIN
	DECLARE @SmallestRow INT, @LargestRow INT

	SET @SmallestRow = @StartRecord + 1
	SET @LargestRow = @SmallestRow + @Length - 1 
	;
	WITH TempTB AS (
		SELECT ROW_NUMBER() OVER (ORDER BY 
									CASE WHEN (@OrderBy = 'JobNumber' AND @OrderType='ASC')
										THEN JobNumber
									END ASC,
									CASE WHEN (@OrderBy = 'JobNumber' AND @OrderType = 'DESC')
										THEN JobNumber
									END DESC,
									CASE WHEN (@OrderBy = 'Location' AND @OrderType='ASC')
										THEN Location
									END ASC,
									CASE WHEN (@OrderBy = 'Location' AND @OrderType = 'DESC')
										THEN Location
									END DESC,
									CASE WHEN (@OrderBy = 'JobType' AND @OrderType='ASC')
										THEN JobType
									END ASC,
									CASE WHEN (@OrderBy = 'JobType' AND @OrderType = 'DESC')
										THEN JobType
									END DESC,
									CASE WHEN (@OrderBy = 'AddedDate' AND @OrderType='ASC')
										THEN AddedDate
									END ASC,
									CASE WHEN (@OrderBy = 'AddedDate' AND @OrderType = 'DESC')
										THEN AddedDate
									END DESC,
									CASE WHEN (@OrderBy = 'FinalCleanDate' AND @OrderType='ASC')
										THEN FinalCleanDate
									END ASC,
									CASE WHEN (@OrderBy = 'FinalCleanDate' AND @OrderType = 'DESC')
										THEN FinalCleanDate
									END DESC,
									CASE WHEN (@OrderBy = 'Area' AND @OrderType='ASC')
										THEN QuadArea
									END ASC,
									CASE WHEN (@OrderBy = 'Area' AND @OrderType = 'DESC')
										THEN QuadArea
									END DESC,
									CASE WHEN (@OrderBy = 'ConfirmedDate' AND @OrderType='ASC')
										THEN ConfirmLostJobDate
									END ASC,
									CASE WHEN (@OrderBy = 'ConfirmedDate' AND @OrderType = 'DESC')
										THEN ConfirmLostJobDate
									END DESC,
									CASE WHEN (@OrderBy = 'Overdue' AND @OrderType='ASC')
										THEN Overdue
									END ASC,
									CASE WHEN (@OrderBy = 'Overdue' AND @OrderType = 'DESC')
										THEN Overdue
									END DESC

											
		) AS rowNumber,
		Count(*) over () AS TotalCount,
		 ID,
		 JobNumber, 
		 Location,
		 QuadArea,
		 JobType,
		 CONVERT(NVARCHAR(50), AddedDate, 103) AS AddedDate, 
		 CONVERT(NVARCHAR(50), FinalCleanDate, 103) AS FinalCleanDate,
		 CONVERT(NVARCHAR(50), ConfirmLostJobDate, 103) AS ConfirmLostJobDate,
		 CEStatus,
		 ContractsStatus,
		 DAStatus,
		 WPSStatus,
		 ComplianceStatus,
		 AOStatus,
		 ARStatus,
		 ConfirmStatus,
		 Overdue
		FROM dbo.LostJobs 
		WHERE	((@LostJobConfirmed = 'All' OR @LostJobConfirmed IS NULL) OR (ConfirmStatus=@LostJobConfirmed)) AND 
				(@JobNumber IS NULL OR @JobNumber = '' OR JobNumber = @JobNumber) AND
				(@Location IS NULL OR @Location = '' OR  LOWER(Location) LIKE '%'+LOWER(@Location)+'%') AND
				(@FinalCleanStart IS NULL OR FinalCleanDate >= @FinalCleanStart) AND
				(@FinalCleanEnd IS NULL OR FinalCleanDate <= @FinalCleanEnd ) AND
				(@AddedDateStart IS NULL OR AddedDate >= @AddedDateStart) AND
				(@AddedDateEnd IS NULL OR AddedDate <= @AddedDateEnd) AND
				(@ConfirmDateStart IS NULL OR ConfirmLostJobDate >= @ConfirmDateStart) AND
				(@ConfirmDateEnd IS NULL OR ConfirmLostJobDate <= @ConfirmDateEnd) AND
				((@DA = 'All' OR @DA IS NULL) OR (DAStatus = @DA)) AND
				((@Compliance = 'All' OR @Compliance IS NULL) OR (ComplianceStatus = @Compliance)) AND
				((@Accounts = 'All' OR @Accounts IS NULL) OR (AOStatus = @Accounts)) AND
				((@Contracts = 'All' OR @Contracts IS NULL) OR (ContractsStatus = @Contracts)) AND
				((@CE = 'All' OR @CE IS NULL) OR (CEStatus = @CE)) AND
				((@CC = 'All' OR @CC IS NULL) OR (WPSStatus = @CC)) AND
				((@AR = 'All' OR @AR IS NULL) OR (ARStatus = @AR)))
	SELECT * FROM TempTB AS TB
	WHERE rowNumber >= @SmallestRow AND rowNumber <= @LargestRow
	ORDER BY rowNumber ASC
END    
