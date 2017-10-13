CREATE TABLE characters
(
  CID            BIGINT AUTO_INCREMENT
    PRIMARY KEY,
  UID            BIGINT                        NOT NULL,
  Name           VARCHAR(21)                   NOT NULL,
  CreationDate   INT(8)                        NOT NULL,
  Mito           BIGINT DEFAULT '1000'         NULL,
  Avatar         INT DEFAULT '0'               NULL,
  Level          INT DEFAULT '1'               NULL,
  BaseExp        INT DEFAULT '0'               NULL,
  CurExp         INT DEFAULT '0'               NULL,
  NextExp        INT DEFAULT '100'             NULL,
  City           INT DEFAULT '1'               NULL
  COMMENT '0 = driverDome, 1 = moonpalace',
  CurrentCarID   INT DEFAULT '1'               NULL,
  GarageLevel    INT DEFAULT '0'               NULL,
  InventoryLevel INT DEFAULT '0'               NULL,
  posX           DOUBLE(11, 3) DEFAULT '0.000' NULL,
  posY           DOUBLE(11, 3) DEFAULT '0.000' NULL,
  posZ           DOUBLE(11, 3) DEFAULT '0.000' NULL,
  posW           DOUBLE(11, 3) DEFAULT '0.000' NULL,
  channelId      INT                           NULL,
  posState       INT DEFAULT '0'               NULL,
  Mileage        BIGINT DEFAULT '0'            NULL,
  TeamId         BIGINT DEFAULT '-1'           NULL,
  TeamRank       INT(18) DEFAULT '-1'          NULL,
  Guild          INT(1) DEFAULT '0'            NULL,
  Hancoin        INT DEFAULT '0'               NULL
);

CREATE INDEX UID
  ON characters (UID);

CREATE TABLE friends
(
  SERVERID INT(18)          NOT NULL,
  CID      INT(18)          NOT NULL,
  FCID     INT(18)          NOT NULL,
  FSTATE   CHAR DEFAULT 'F' NULL,
  PRIMARY KEY (SERVERID, CID, FCID)
);

CREATE TABLE items
(
  Id             INT(12) AUTO_INCREMENT
    PRIMARY KEY,
  CharacterId    INT                 NULL,
  InventoryIndex INT                 NULL,
  StackNum       INT DEFAULT '1'     NULL,
  CarId          INT                 NULL,
  Durability     FLOAT DEFAULT '100' NULL,
  Slot           INT DEFAULT '0'     NULL,
  TableIndex     INT(6)              NULL,
  Random         INT(6) DEFAULT '0'  NULL,
  UpgradePoint   INT(6) DEFAULT '0'  NULL,
  Upgrade        INT(6) DEFAULT '0'  NULL,
  Belonging      INT(6) DEFAULT '0'  NULL,
  Box            INT(6) DEFAULT '0'  NULL,
  AssistJ        INT(6) DEFAULT '0'  NULL,
  AssistI        INT(6) DEFAULT '0'  NULL,
  AssistH        INT(6) DEFAULT '0'  NULL,
  AssistG        INT(6) DEFAULT '0'  NULL,
  AssistF        INT(6) DEFAULT '0'  NULL,
  AssistE        INT(6) DEFAULT '0'  NULL,
  AssistD        INT(6) DEFAULT '0'  NULL,
  AssistC        INT(6) DEFAULT '0'  NULL,
  AssistB        INT(6) DEFAULT '0'  NULL,
  AssistA        INT(6) DEFAULT '0'  NULL,
  State          INT(6) DEFAULT '0'  NULL
);

CREATE TABLE quests
(
  ServerId INT(18)     NOT NULL,
  CID      INT(18)     NOT NULL,
  CNAME    VARCHAR(32) NOT NULL,
  QuestId  VARCHAR(20) NOT NULL,
  State    INT(18)     NOT NULL,
  FailNum  INT(18)     NOT NULL,
  PlaceIdx INT(18)     NOT NULL,
  LastDate INT(255)    NULL,
  PRIMARY KEY (ServerId, CID, QuestId)
);

CREATE TABLE servers
(
  Id                 INT(255) AUTO_INCREMENT
    PRIMARY KEY,
  Name               VARCHAR(255) DEFAULT 'Test'      NOT NULL,
  PlayersOnline      INT(5) DEFAULT '0'               NULL,
  MaxPlayers         INT(5) DEFAULT '10'              NULL,
  GameServerIp       VARCHAR(255) DEFAULT '127.0.0.1' NULL,
  GameServerPort     INT(5) DEFAULT '11021'           NULL,
  LobbyServerIp      VARCHAR(255) DEFAULT '127.0.0.1' NULL,
  LobbyServerPort    INT(5) DEFAULT '11011'           NULL,
  AreaServer1Ip      VARCHAR(255) DEFAULT '127.0.0.1' NULL,
  AreaServer1UdpPort INT(5) DEFAULT '10701'           NULL,
  AreaServer1Port    INT(5) DEFAULT '11031'           NULL,
  AreaServer2Ip      VARCHAR(255) DEFAULT '127.0.0.1' NULL,
  AreaServer2UdpPort INT(5) DEFAULT '10702'           NULL,
  AreaServer2Port    INT(5) DEFAULT '11041'           NULL,
  RankingServerIp    VARCHAR(255) DEFAULT '127.0.0.1' NULL,
  RankingServerPort  INT(5) DEFAULT '11078'           NULL
);

CREATE TABLE shop
(
  ItemID BIGINT NOT NULL,
  Price  INT    NOT NULL
);

CREATE TABLE teams
(
  SERVERID       INT(18)                  NOT NULL,
  TID            INT(18) AUTO_INCREMENT,
  TMARKID        DECIMAL(18) DEFAULT '-1' NULL,
  TEAMNAME       VARCHAR(16)              NOT NULL,
  UTEAMNAME      VARCHAR(16)              NOT NULL,
  TEAMDESC       VARCHAR(80)              NULL,
  TEAMLEVEL      DECIMAL(18) DEFAULT '0'  NULL,
  TEAMPOINT      DECIMAL(18) DEFAULT '0'  NULL,
  TEAMRANKING    DECIMAL(18) DEFAULT '0'  NULL,
  LEFTNEXP       DECIMAL(18) DEFAULT '0'  NULL,
  LEFTPLAYTIME   DECIMAL(18) DEFAULT '0'  NULL,
  LEFTITEMVAL    DECIMAL(18) DEFAULT '0'  NULL,
  CHANNELWINCNT  DECIMAL(18) DEFAULT '0'  NULL,
  MEMBERCNT      DECIMAL(18) DEFAULT '0'  NULL,
  TEAMGRADE      CHAR                     NULL,
  TEAMTOTALPOINT DECIMAL(18)              NULL,
  TAXINCOME      DECIMAL(18) DEFAULT '0'  NULL,
  CID            DECIMAL(18)              NOT NULL,
  CNAME          VARCHAR(32)              NOT NULL,
  OWNCHANNEL     VARCHAR(40)              NULL,
  TEAMSTATE      CHAR DEFAULT 'A'         NULL,
  CREATEDATE     INT(8) DEFAULT '0'       NULL,
  CLOSEDATE      INT(8) DEFAULT '0'       NULL,
  BANISHDATE     INT(8) DEFAULT '0'       NULL,
  TEAMURL        VARCHAR(32)              NULL,
  UTEAMURL       VARCHAR(32)              NULL,
  LASTDATE       INT(8)                   NULL,
  PRIMARY KEY (TID, SERVERID)
);

CREATE TABLE updates
(
  path VARCHAR(255) NOT NULL
    PRIMARY KEY
);

CREATE TABLE users
(
  UID            BIGINT AUTO_INCREMENT
    PRIMARY KEY,
  Username       VARCHAR(21)                     NOT NULL,
  Password       VARCHAR(64)                     NOT NULL,
  Salt           VARCHAR(64)                     NOT NULL,
  Ticket         INT(20) UNSIGNED                NOT NULL,
  Status         TINYINT DEFAULT '1'             NOT NULL,
  CreateIP       VARCHAR(15) DEFAULT '127.0.0.1' NOT NULL,
  CreateDate     BIGINT DEFAULT '0'              NOT NULL,
  Permission     INT(6) DEFAULT '0'              NOT NULL,
  LastActiveChar INT(6) DEFAULT '0'              NULL,
  BanValidUntil  BIGINT DEFAULT '0'              NULL,
  VehicleSerial  INT(6) UNSIGNED DEFAULT '0'     NULL
);

ALTER TABLE characters
  ADD CONSTRAINT characters_ibfk_1
FOREIGN KEY (UID) REFERENCES dcmm.users (UID);

CREATE TABLE vehicles
(
  CID              BIGINT AUTO_INCREMENT
    PRIMARY KEY,
  CharID           BIGINT                         NOT NULL,
  auctionCount     INT DEFAULT '0'                NOT NULL,
  baseColor        INT DEFAULT '0'                NOT NULL,
  carType          INT DEFAULT '24'               NOT NULL,
  grade            INT DEFAULT '9'                NOT NULL,
  mitron           DOUBLE(11, 2) DEFAULT '0.00'   NOT NULL,
  kmh              DOUBLE(11, 2) DEFAULT '0.00'   NOT NULL,
  slotType         INT DEFAULT '0'                NOT NULL,
  color            INT DEFAULT '0'                NOT NULL,
  mitronCapacity   DOUBLE(11, 2) DEFAULT '500.00' NOT NULL,
  mitronEfficiency DOUBLE(11, 2) DEFAULT '0.00'   NOT NULL
);

