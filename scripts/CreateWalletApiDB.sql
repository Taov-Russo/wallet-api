USE [master]
GO

CREATE DATABASE [WalletApiDB]
GO

USE [WalletApiDB]
GO

CREATE TABLE [Wallet] (
	[WalletId] [uniqueidentifier] NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[Balance] [decimal](9,2) NOT NULL
	CONSTRAINT [PK_Wallet] PRIMARY KEY CLUSTERED 
	(
		[WalletId] ASC
	)
)
GO

CREATE TABLE [Transaction](
	[TransactionId] [uniqueidentifier] NOT NULL,
	[WalletId] [uniqueidentifier] NOT NULL,
	[Amount] [decimal](9,2) NOT NULL,
	[OperationType] [int] NOT NULL,
	CONSTRAINT [PK_Transaction] PRIMARY KEY CLUSTERED 
	(
		[TransactionId] ASC
	)
)
GO

ALTER TABLE [Transaction] WITH CHECK
	ADD CONSTRAINT [FK_TransactionToWallet] FOREIGN KEY ([WalletId])
	REFERENCES [Wallet] ([WalletId])
GO

ALTER TABLE [Wallet]
ADD CONSTRAINT CK_Balance CHECK (Balance >= 0)

ALTER TABLE [Transaction]
ADD CONSTRAINT CK_Amount CHECK (Amount >= 0)