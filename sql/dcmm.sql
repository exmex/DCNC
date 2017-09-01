SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for characters
-- ----------------------------
DROP TABLE IF EXISTS `characters`;
CREATE TABLE `characters` (
  `CID` bigint(20) NOT NULL AUTO_INCREMENT,
  `UID` bigint(20) NOT NULL,
  `Name` varchar(21) NOT NULL,
  `CreationDate` int(8) NOT NULL,
  `Mito` bigint(20) NULL DEFAULT '1000',
  `Mileage` bigint(20) NULL DEFAULT '0',
  `Avatar` int(11) NULL DEFAULT '0',
  `Level` int(11) NULL DEFAULT '1',
  `BaseExp` int(11) NULL DEFAULT '0',
  `CurExp` int(11) NULL DEFAULT '0',
  `NextExp` int(11) NULL DEFAULT '100',
  `City` int(11) NULL DEFAULT '1' COMMENT '0 = driverDome, 1 = moonpalace',
  `CurrentCarID` int(11) NULL DEFAULT '1',
  `GarageLevel` int(11) NULL DEFAULT '1',
  `TeamId` bigint(20) NULL DEFAULT '-1',
  `TeamRank` int(18) NULL DEFAULT '-1',
  `InventoryLevel` int(11) NULL,
  `posX` double(11,3) NULL DEFAULT '0',
  `posY` double(11,3) NULL DEFAULT '0',
  `posZ` double(11,3) NULL DEFAULT '0',
  `posW` double(11,3) NULL DEFAULT '0',
  `channelId` int(11) NULL,
  `posState` int(11) NULL DEFAULT '0',
  PRIMARY KEY (`CID`),
  KEY `UID` (`UID`),
  CONSTRAINT `characters_ibfk_1` FOREIGN KEY (`UID`) REFERENCES `users` (`UID`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;

-- ----------------------------
-- Table structure for friends
-- ----------------------------
DROP TABLE IF EXISTS `friends`;
CREATE TABLE `friends` (
  `SERVERID` int(18) NOT NULL,
  `CID` int(18) NOT NULL,
  `FCID` int(18) NOT NULL,
  `FSTATE` char(1) DEFAULT 'F',
  PRIMARY KEY (`SERVERID`,`CID`,`FCID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of friends
-- ----------------------------

-- ----------------------------
-- Table structure for quests
-- ----------------------------
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

-- ----------------------------
-- Table structure for servers
-- ----------------------------
DROP TABLE IF EXISTS `servers`;
CREATE TABLE `servers` (
  `Id` int(255) NOT NULL AUTO_INCREMENT,
  `Name` varchar(255) NOT NULL DEFAULT 'Test',
  `PlayersOnline` int(5) DEFAULT '0',
  `MaxPlayers` int(5) DEFAULT '10',
  `GameServerIp` varchar(255) DEFAULT '127.0.0.1',
  `GameServerPort` int(5) DEFAULT '11021',
  `LobbyServerIp` varchar(255) DEFAULT '127.0.0.1',
  `LobbyServerPort` int(5) DEFAULT '11011',
  `AreaServer1Ip` varchar(255) DEFAULT '127.0.0.1',
  `AreaServer1UdpPort` int(5) DEFAULT '10701',
  `AreaServer1Port` int(5) DEFAULT '11031',
  `AreaServer2Ip` varchar(255) DEFAULT '127.0.0.1',
  `AreaServer2UdpPort` int(5) DEFAULT '10702',
  `AreaServer2Port` int(5) DEFAULT '11041',
  `RankingServerIp` varchar(255) DEFAULT '127.0.0.1',
  `RankingServerPort` int(5) DEFAULT '11078',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of servers
-- ----------------------------
INSERT INTO `servers` VALUES ('1', 'Test', '0', '10', '127.0.0.1', '11021', '127.0.0.1', '11011', '127.0.0.1', '10701', '11031', '127.0.0.1', '10702', '11041', '127.0.0.1', '11078');

-- ----------------------------
-- Table structure for shop
-- ----------------------------
DROP TABLE IF EXISTS `shop`;
CREATE TABLE `shop` (
  `ItemID` bigint(20) NOT NULL,
  `Price` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of shop
-- ----------------------------
INSERT INTO `shop` VALUES ('0', '250');
INSERT INTO `shop` VALUES ('5', '400');
INSERT INTO `shop` VALUES ('10', '1050');
INSERT INTO `shop` VALUES ('15', '2000');
INSERT INTO `shop` VALUES ('20', '3250');
INSERT INTO `shop` VALUES ('25', '250');
INSERT INTO `shop` VALUES ('30', '400');
INSERT INTO `shop` VALUES ('35', '1050');
INSERT INTO `shop` VALUES ('40', '2000');
INSERT INTO `shop` VALUES ('45', '3250');
INSERT INTO `shop` VALUES ('50', '250');
INSERT INTO `shop` VALUES ('55', '400');
INSERT INTO `shop` VALUES ('60', '1050');
INSERT INTO `shop` VALUES ('65', '2000');
INSERT INTO `shop` VALUES ('70', '3250');
INSERT INTO `shop` VALUES ('75', '250');
INSERT INTO `shop` VALUES ('80', '400');
INSERT INTO `shop` VALUES ('85', '1050');
INSERT INTO `shop` VALUES ('90', '2000');
INSERT INTO `shop` VALUES ('95', '3250');
INSERT INTO `shop` VALUES ('1445', '5000');
INSERT INTO `shop` VALUES ('1488', '50');
INSERT INTO `shop` VALUES ('1502', '200');
INSERT INTO `shop` VALUES ('1503', '1000');
INSERT INTO `shop` VALUES ('1504', '30000');
INSERT INTO `shop` VALUES ('1516', '5000');
INSERT INTO `shop` VALUES ('1554', '100');
INSERT INTO `shop` VALUES ('1561', '1000');
INSERT INTO `shop` VALUES ('1568', '500');
INSERT INTO `shop` VALUES ('1569', '1000');
INSERT INTO `shop` VALUES ('1570', '2000');
INSERT INTO `shop` VALUES ('1665', '1000');
INSERT INTO `shop` VALUES ('1666', '1000');
INSERT INTO `shop` VALUES ('1667', '1000');
INSERT INTO `shop` VALUES ('1818', '3000');
INSERT INTO `shop` VALUES ('1874', '49000');
INSERT INTO `shop` VALUES ('1875', '63000');
INSERT INTO `shop` VALUES ('1876', '98000');
INSERT INTO `shop` VALUES ('1877', '196000');
INSERT INTO `shop` VALUES ('2546', '4000');
INSERT INTO `shop` VALUES ('2547', '6000');
INSERT INTO `shop` VALUES ('2548', '7000');
INSERT INTO `shop` VALUES ('1979', '1200');
INSERT INTO `shop` VALUES ('1980', '700');
INSERT INTO `shop` VALUES ('1981', '2000');
INSERT INTO `shop` VALUES ('1982', '100');
INSERT INTO `shop` VALUES ('2032', '1000');
INSERT INTO `shop` VALUES ('1989', '10000');
INSERT INTO `shop` VALUES ('2013', '15000');
INSERT INTO `shop` VALUES ('2014', '15000');
INSERT INTO `shop` VALUES ('2015', '15000');
INSERT INTO `shop` VALUES ('2034', '10000');
INSERT INTO `shop` VALUES ('2068', '5000');
INSERT INTO `shop` VALUES ('2069', '7000');
INSERT INTO `shop` VALUES ('2070', '10000');
INSERT INTO `shop` VALUES ('2700', '10000');
INSERT INTO `shop` VALUES ('2708', '10000');
INSERT INTO `shop` VALUES ('2709', '10000');
INSERT INTO `shop` VALUES ('2003', '10000');
INSERT INTO `shop` VALUES ('2004', '12000');
INSERT INTO `shop` VALUES ('2005', '15000');
INSERT INTO `shop` VALUES ('2031', '12000');
INSERT INTO `shop` VALUES ('2036', '20000');
INSERT INTO `shop` VALUES ('2025', '30000');

-- ----------------------------
-- Table structure for teams
-- ----------------------------
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

-- ----------------------------
-- Records of teams
-- ----------------------------
INSERT INTO `teams` VALUES ('0', '0', '1', 'Staff', 'Staff', 'Staff Only team', '60', '999', '10', '0', '0', '0', '0', '1', null, null, '0', '1', '', null, 'A', '0', '0', '0', 'http://placehold.it/350x150', 'http://placehold.it/350x150', '0');

-- ----------------------------
-- Table structure for updates
-- ----------------------------
DROP TABLE IF EXISTS `updates`;
CREATE TABLE `updates` (
  `path` varchar(255) NOT NULL,
  PRIMARY KEY (`path`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of updates
-- ----------------------------
INSERT INTO `updates` VALUES ('main.sql');

-- ----------------------------
-- Table structure for users
-- ----------------------------
DROP TABLE IF EXISTS `users`;
CREATE TABLE `users` (
  `UID` bigint(20) NOT NULL AUTO_INCREMENT,
  `Username` varchar(21) NOT NULL,
  `Password` varchar(64) NOT NULL,
  `Salt` varchar(64) NOT NULL,
  `Permission` int(6) DEFAULT 0 NOT NULL,
  `Ticket` int(20) unsigned NOT NULL,
  `Status` tinyint(4) NOT NULL DEFAULT '1',
  `CreateIP` varchar(15) NOT NULL DEFAULT '127.0.0.1',
  `CreateDate` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`UID`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;

-- ----------------------------
-- Table structure for vehicles
-- ----------------------------
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
