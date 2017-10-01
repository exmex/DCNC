SET FOREIGN_KEY_CHECKS=0;

DROP TABLE IF EXISTS `characters`;
CREATE TABLE `characters` (
  `CID` bigint(20) NOT NULL AUTO_INCREMENT,
  `UID` bigint(20) NOT NULL,
  `Name` varchar(21) NOT NULL,
  `CreationDate` int(8) NOT NULL,
  `Mito` bigint(20) DEFAULT '1000',
  `Mileage` bigint(20) DEFAULT '0',
  `Avatar` int(11) DEFAULT '0',
  `Guild` int(1) DEFAULT '0',
  `Level` int(11) DEFAULT '1',
  `BaseExp` int(11) DEFAULT '0',
  `CurExp` int(11) DEFAULT '0',
  `NextExp` int(11) DEFAULT '100',
  `City` int(11) DEFAULT '1' COMMENT '0 = driverDome, 1 = moonpalace',
  `CurrentCarID` int(11) DEFAULT '0',
  `GarageLevel` int(11) DEFAULT '0',
  `TeamId` bigint(20) DEFAULT '-1',
  `TeamRank` int(18) DEFAULT '-1',
  `InventoryLevel` int(11) DEFAULT '0',
  `posX` double(11,3) DEFAULT '0.000',
  `posY` double(11,3) DEFAULT '0.000',
  `posZ` double(11,3) DEFAULT '0.000',
  `posW` double(11,3) DEFAULT '0.000',
  `channelId` int(11) DEFAULT NULL,
  `posState` int(11) DEFAULT '0',
  PRIMARY KEY (`CID`),
  KEY `UID` (`UID`),
  CONSTRAINT `characters_ibfk_1` FOREIGN KEY (`UID`) REFERENCES `users` (`UID`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;

DROP TABLE IF EXISTS `friends`;
CREATE TABLE `friends` (
  `SERVERID` int(18) NOT NULL,
  `CID` int(18) NOT NULL,
  `FCID` int(18) NOT NULL,
  `FSTATE` char(1) DEFAULT 'F',
  PRIMARY KEY (`SERVERID`,`CID`,`FCID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

DROP TABLE IF EXISTS `items`;
CREATE TABLE `items` (
  `Id` int(12) NOT NULL AUTO_INCREMENT,
  `CarId` int(11) DEFAULT NULL,
  `Random` int(6) DEFAULT '0',
  `CharacterId` int(11) DEFAULT NULL,
  `Durability` float DEFAULT '100',
  `TableIndex` int(6) DEFAULT NULL,
  `InventoryIndex` int(11) DEFAULT NULL,
  `UpgradePoint` int(6) DEFAULT '0',
  `Upgrade` int(6) DEFAULT '0',
  `Belonging` int(6) DEFAULT '0',
  `Box` int(6) DEFAULT '0',
  `AssistJ` int(6) DEFAULT '0',
  `AssistI` int(6) DEFAULT '0',
  `AssistH` int(6) DEFAULT '0',
  `AssistG` int(6) DEFAULT '0',
  `AssistF` int(6) DEFAULT '0',
  `AssistE` int(6) DEFAULT '0',
  `AssistD` int(6) DEFAULT '0',
  `AssistC` int(6) DEFAULT '0',
  `AssistB` int(6) DEFAULT '0',
  `AssistA` int(6) DEFAULT '0',
  `State` int(6) DEFAULT '0',
  `Slot` int(11) DEFAULT '0',
  `StackNum` int(11) DEFAULT '1',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;

DROP TABLE IF EXISTS `quests`;
CREATE TABLE `quests` (
  `ServerId` int(18) NOT NULL,
  `CID` int(18) NOT NULL,
  `CNAME` varchar(32) NOT NULL,
  `QuestId` varchar(20) NOT NULL,
  `State` int(18) NOT NULL,
  `FailNum` int(18) NOT NULL,
  `PlaceIdx` int(18) NOT NULL,
  `LastDate` int(255) DEFAULT NULL,
  PRIMARY KEY (`ServerId`,`CID`,`QuestId`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

DROP TABLE IF EXISTS `shop`;
CREATE TABLE `shop` (
  `ItemID` bigint(20) NOT NULL,
  `Price` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

DROP TABLE IF EXISTS `teams`;
CREATE TABLE `teams` (
  `SERVERID` decimal(18,0) NOT NULL,
  `TID` decimal(18,0) NOT NULL,
  `TMARKID` decimal(18,0) DEFAULT '-1',
  `TEAMNAME` varchar(16) NOT NULL,
  `UTEAMNAME` varchar(16) NOT NULL,
  `TEAMDESC` varchar(80) DEFAULT NULL,
  `TEAMLEVEL` decimal(18,0) DEFAULT '0',
  `TEAMPOINT` decimal(18,0) DEFAULT '0',
  `TEAMRANKING` decimal(18,0) DEFAULT '0',
  `LEFTNEXP` decimal(18,0) DEFAULT '0',
  `LEFTPLAYTIME` decimal(18,0) DEFAULT '0',
  `LEFTITEMVAL` decimal(18,0) DEFAULT '0',
  `CHANNELWINCNT` decimal(18,0) DEFAULT '0',
  `MEMBERCNT` decimal(18,0) DEFAULT '0',
  `TEAMGRADE` char(1) DEFAULT NULL,
  `TEAMTOTALPOINT` decimal(18,0) DEFAULT NULL,
  `TAXINCOME` decimal(18,0) DEFAULT '0',
  `CID` decimal(18,0) NOT NULL,
  `CNAME` varchar(32) NOT NULL,
  `OWNCHANNEL` varchar(40) DEFAULT NULL,
  `TEAMSTATE` char(1) DEFAULT 'A',
  `CREATEDATE` int(8) DEFAULT '0',
  `CLOSEDATE` int(8) DEFAULT '0',
  `BANISHDATE` int(8) DEFAULT '0',
  `TEAMURL` varchar(32) DEFAULT NULL,
  `UTEAMURL` varchar(32) DEFAULT NULL,
  `LASTDATE` int(8) DEFAULT NULL,
  PRIMARY KEY (`TID`,`SERVERID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

DROP TABLE IF EXISTS `updates`;
CREATE TABLE `updates` (
  `path` varchar(255) NOT NULL,
  PRIMARY KEY (`path`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

DROP TABLE IF EXISTS `users`;
CREATE TABLE `users` (
  `UID` bigint(20) NOT NULL AUTO_INCREMENT,
  `Username` varchar(21) NOT NULL,
  `Password` varchar(64) NOT NULL,
  `Salt` varchar(64) NOT NULL,
  `Permission` int(6) NOT NULL DEFAULT '0',
  `Ticket` int(20) unsigned NOT NULL,
  `Status` tinyint(4) NOT NULL DEFAULT '1',
  `CreateIP` varchar(15) NOT NULL DEFAULT '127.0.0.1',
  `CreateDate` bigint(20) NOT NULL DEFAULT '0',
  `LastActiveChar` int(6) DEFAULT '0',
  `VehicleSerial` int(6) unsigned DEFAULT '0',
  `BanValidUntil` bigint(20) DEFAULT '0',
  PRIMARY KEY (`UID`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;

DROP TABLE IF EXISTS `vehicles`;
CREATE TABLE `vehicles` (
  `CID` bigint(20) NOT NULL AUTO_INCREMENT,
  `CharID` bigint(20) NOT NULL,
  `auctionCount` int(11) NOT NULL DEFAULT '0',
  `baseColor` int(11) NOT NULL DEFAULT '0',
  `carType` int(11) NOT NULL DEFAULT '24',
  `grade` int(11) NOT NULL DEFAULT '9',
  `mitron` double(11,2) NOT NULL DEFAULT '0.00',
  `kmh` double(11,2) NOT NULL DEFAULT '0.00',
  `slotType` int(11) NOT NULL DEFAULT '0',
  `color` int(11) NOT NULL DEFAULT '0',
  `mitronCapacity` double(11,2) NOT NULL DEFAULT '500.00',
  `mitronEfficiency` double(11,2) NOT NULL DEFAULT '0.00',
  PRIMARY KEY (`CID`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;
SET FOREIGN_KEY_CHECKS=1;
