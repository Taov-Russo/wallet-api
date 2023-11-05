USE [master]
GO
CREATE LOGIN [wallet_api] WITH PASSWORD=N'wallet_api', DEFAULT_DATABASE=[master], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
GO
USE [WalletApiDB]
GO
CREATE USER [wallet_api] FOR LOGIN [wallet_api]
GO
USE [WalletApiDB]
GO
ALTER ROLE [db_owner] ADD MEMBER [wallet_api]
GO