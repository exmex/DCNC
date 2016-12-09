/*
Navicat MySQL Data Transfer

Source Server         : USBW
Source Server Version : 50613
Source Host           : localhost:3306
Source Database       : dcmm

Target Server Type    : MYSQL
Target Server Version : 50613
File Encoding         : 65001

Date: 2016-12-01 01:47:41
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for characters
-- ----------------------------
DROP TABLE IF EXISTS `characters`;
CREATE TABLE `characters` (
  `CID` bigint(20) NOT NULL AUTO_INCREMENT,
  `UID` bigint(20) NOT NULL,
  `Name` varchar(21) NOT NULL,
  `Mito` bigint(20) NOT NULL DEFAULT '1000',
  `Avatar` int(11) NOT NULL DEFAULT '1',
  `Level` int(11) NOT NULL DEFAULT '1',
  `City` int(11) NOT NULL DEFAULT '1',
  `CurrentCarID` int(11) NOT NULL DEFAULT '1',
  `GarageLevel` int(11) NOT NULL DEFAULT '1',
  `TID` bigint(20) NOT NULL DEFAULT '0',
  `InventoryLevel` int(11) NOT NULL,
  PRIMARY KEY (`CID`),
  KEY `UID` (`UID`),
  CONSTRAINT `characters_ibfk_1` FOREIGN KEY (`UID`) REFERENCES `users` (`UID`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of characters
-- ----------------------------
INSERT INTO `characters` VALUES ('1', '1', 'Administrator', '1000', '1', '99', '1', '1', '1', '0', '0');
INSERT INTO `characters` VALUES ('2', '1', 'Admin', '123456', '2', '15', '1', '2', '1', '0', '0');

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
  `PasswordHash` varchar(32) NOT NULL,
  `Status` tinyint(4) NOT NULL DEFAULT '1',
  `CreateIP` varchar(15) NOT NULL DEFAULT '127.0.0.1',
  `CreateDate` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`UID`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of users
-- ----------------------------
INSERT INTO `users` VALUES ('1', 'admin', '21232f297a57a5a743894a0e4a801fc3', '1', '127.0.0.1', '2014-05-01 17:55:42');

-- ----------------------------
-- Table structure for vehicles
-- ----------------------------
DROP TABLE IF EXISTS `vehicles`;
CREATE TABLE `vehicles` (
  `CID` bigint(20) NOT NULL,
  `CharID` bigint(20) NOT NULL,
  `auctionCount` int(11) NOT NULL,
  `baseColor` int(11) NOT NULL,
  `carType` int(11) NOT NULL,
  `grade` int(11) NOT NULL,
  `mitron` double(11,2) NOT NULL,
  `kmh` double(11,2) NOT NULL,
  `slotType` int(11) NOT NULL,
  `color` int(11) NOT NULL,
  `mitronCapacity` double(11,2) NOT NULL,
  `mitronEfficiency` double(11,2) NOT NULL,
  PRIMARY KEY (`CID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of vehicles
-- ----------------------------
INSERT INTO `vehicles` VALUES ('1', '1', '0', '0', '24', '9', '0.00', '200.00', '0', '0', '0.00', '0.00');
INSERT INTO `vehicles` VALUES ('2', '2', '0', '0', '24', '9', '0.00', '200.00', '0', '0', '0.00', '0.00');
