✅ Full Stored Procedure: ManageBrand
CREATE PROCEDURE ManageBrand
    @Action NVARCHAR(20),
    @BrandId INT = NULL,
    @BrandName NVARCHAR(100) = NULL,
    @CompanyLogo NVARCHAR(255) = NULL,
    @CreatedAt DATETIME = NULL,
    @UpdatedAt DATETIME = NULL,
    @IsActive BIT = 1
AS
BEGIN
    SET NOCOUNT ON;

    -- CREATE
    IF @Action = 'INSERT'
    BEGIN
        INSERT INTO BrandInfo (BrandName, CompanyLogo, CreatedAt, UpdatedAt, IsActive)
        VALUES (@BrandName, @CompanyLogo, GETDATE(), NULL, 1);
    END

    -- READ ALL ACTIVE
    ELSE IF @Action = 'SELECT_ALL'
    BEGIN
        SELECT * FROM BrandInfo WHERE IsActive = 1 ORDER BY BrandId DESC;
    END

    -- READ SINGLE
    ELSE IF @Action = 'SELECT_BY_ID'
    BEGIN
        SELECT * FROM BrandInfo WHERE BrandId = @BrandId;
    END

    -- UPDATE
    ELSE IF @Action = 'UPDATE'
    BEGIN
        UPDATE BrandInfo
        SET BrandName = @BrandName,
            CompanyLogo = @CompanyLogo,
            UpdatedAt = GETDATE()
        WHERE BrandId = @BrandId;
    END

    -- DELETE (Soft Delete)
    ELSE IF @Action = 'DELETE'
    BEGIN
        UPDATE BrandInfo
        SET IsActive = 0, UpdatedAt = GETDATE()
        WHERE BrandId = @BrandId;
    END
END
