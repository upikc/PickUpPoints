-- MySQL dump 10.13  Distrib 8.0.38, for Win64 (x86_64)
--
-- Host: 127.0.0.1    Database: dbstorage
-- ------------------------------------------------------
-- Server version	8.0.39

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `dimensions`
--

DROP TABLE IF EXISTS `dimensions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `dimensions` (
  `dimension_id` varchar(6) NOT NULL,
  `dimension_title` varchar(45) NOT NULL,
  PRIMARY KEY (`dimension_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `dimensions`
--

LOCK TABLES `dimensions` WRITE;
/*!40000 ALTER TABLE `dimensions` DISABLE KEYS */;
INSERT INTO `dimensions` VALUES ('L_box','Стандарт коробка до 530×360×220 мм.'),('m_box','Стандарт коробка до 400×270×180 мм.'),('pack','Мелкий пакет'),('s_box','Стандарт коробка до 260×170×80 мм');
/*!40000 ALTER TABLE `dimensions` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `operation_types`
--

DROP TABLE IF EXISTS `operation_types`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `operation_types` (
  `type_id` int NOT NULL,
  `type` varchar(20) NOT NULL,
  PRIMARY KEY (`type_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `operation_types`
--

LOCK TABLES `operation_types` WRITE;
/*!40000 ALTER TABLE `operation_types` DISABLE KEYS */;
INSERT INTO `operation_types` VALUES (0,'declare'),(1,'transfer'),(2,'received'),(3,'issue');
/*!40000 ALTER TABLE `operation_types` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `packages`
--

DROP TABLE IF EXISTS `packages`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `packages` (
  `package_id` int NOT NULL,
  `weight` decimal(7,2) NOT NULL,
  `unitof_weight_id` int NOT NULL,
  `dimension_id` varchar(6) NOT NULL,
  `sender_Fname` varchar(150) NOT NULL,
  `sender_Sname` varchar(150) NOT NULL,
  `sender_Lname` varchar(150) NOT NULL,
  `sender_mail` varchar(90) NOT NULL,
  `sender_number` varchar(12) DEFAULT NULL,
  `recipient_Fname` varchar(150) NOT NULL,
  `recipient_Sname` varchar(150) NOT NULL,
  `recipient_Lname` varchar(150) NOT NULL,
  `recipient_mail` varchar(90) NOT NULL,
  `recipient_number` varchar(12) DEFAULT NULL,
  PRIMARY KEY (`package_id`),
  KEY `newFK1_idx` (`dimension_id`),
  KEY `newFK2_idx` (`unitof_weight_id`),
  CONSTRAINT `newFK1` FOREIGN KEY (`dimension_id`) REFERENCES `dimensions` (`dimension_id`),
  CONSTRAINT `newFK2` FOREIGN KEY (`unitof_weight_id`) REFERENCES `unitof_weight` (`unitof_weight_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `packages`
--

LOCK TABLES `packages` WRITE;
/*!40000 ALTER TABLE `packages` DISABLE KEYS */;
INSERT INTO `packages` VALUES (0,100.00,0,'pack','Олег','Олегович','Орлов','sender@gmail.com',NULL,'Петр','Петрович','Петухов','uupikc@gmail.com',NULL),(2,20.00,0,'L_box','Октай','Оберучев','Олимпиев','senderww@gmail.com','89373688112','Памела','Пегова','Павловская','uupikc@gmail.com',NULL),(3,100.00,0,'pack','Овидий','Обрядко','Осипович','sender1@gmail.com',NULL,'Павел','Павшин','Панкратович','uupikc@gmail.com',NULL),(4,2.00,1,'s_box','Оксана','Остроушко','Оскаровна','sendermail@gmail.com',NULL,'Пелагея','Павлушкова','Прокопиева ','uupikc@gmail.com',NULL),(5,30.00,1,'L_box','Онисим','Остапов','Олегович','sender4@gmail.com',NULL,'Петр','Петросян','Парфениевич','uupikc@gmail.com',NULL);
/*!40000 ALTER TABLE `packages` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Temporary view structure for view `packages_withstatus`
--

DROP TABLE IF EXISTS `packages_withstatus`;
/*!50001 DROP VIEW IF EXISTS `packages_withstatus`*/;
SET @saved_cs_client     = @@character_set_client;
/*!50503 SET character_set_client = utf8mb4 */;
/*!50001 CREATE VIEW `packages_withstatus` AS SELECT 
 1 AS `package_id`,
 1 AS `weight`,
 1 AS `weight_unit`,
 1 AS `sender_Fname`,
 1 AS `sender_Sname`,
 1 AS `sender_Lname`,
 1 AS `sender_mail`,
 1 AS `sender_number`,
 1 AS `recipient_Fname`,
 1 AS `recipient_Sname`,
 1 AS `recipient_Lname`,
 1 AS `recipient_mail`,
 1 AS `recipient_number`,
 1 AS `dimension_title`,
 1 AS `status`,
 1 AS `status_date`,
 1 AS `actionstorage_id`*/;
SET character_set_client = @saved_cs_client;

--
-- Table structure for table `pkg_operations`
--

DROP TABLE IF EXISTS `pkg_operations`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `pkg_operations` (
  `operation_id` int NOT NULL AUTO_INCREMENT,
  `package_id` int NOT NULL,
  `user_id` int NOT NULL,
  `type_id` int NOT NULL,
  `operation_date` datetime NOT NULL,
  `actionstorage_id` int NOT NULL,
  PRIMARY KEY (`operation_id`),
  KEY `fk6_idx` (`package_id`),
  KEY `fk7_idx` (`type_id`),
  KEY `fk8_idx` (`user_id`),
  KEY `fk9_idx` (`actionstorage_id`),
  CONSTRAINT `fk6` FOREIGN KEY (`package_id`) REFERENCES `packages` (`package_id`),
  CONSTRAINT `fk7` FOREIGN KEY (`type_id`) REFERENCES `operation_types` (`type_id`),
  CONSTRAINT `fk8` FOREIGN KEY (`user_id`) REFERENCES `users` (`user_id`),
  CONSTRAINT `fk9` FOREIGN KEY (`actionstorage_id`) REFERENCES `storages` (`storage_id`)
) ENGINE=InnoDB AUTO_INCREMENT=69 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `pkg_operations`
--

LOCK TABLES `pkg_operations` WRITE;
/*!40000 ALTER TABLE `pkg_operations` DISABLE KEYS */;
INSERT INTO `pkg_operations` VALUES (63,2,1,0,'2025-05-19 04:03:22',1),(64,3,1,0,'2025-05-19 04:06:38',1),(66,4,2,0,'2025-05-19 04:16:08',2),(67,5,1,0,'2025-05-19 04:21:00',1),(68,3,1,1,'2025-05-19 04:21:37',2);
/*!40000 ALTER TABLE `pkg_operations` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Temporary view structure for view `pkg_operations_withtype`
--

DROP TABLE IF EXISTS `pkg_operations_withtype`;
/*!50001 DROP VIEW IF EXISTS `pkg_operations_withtype`*/;
SET @saved_cs_client     = @@character_set_client;
/*!50503 SET character_set_client = utf8mb4 */;
/*!50001 CREATE VIEW `pkg_operations_withtype` AS SELECT 
 1 AS `operation_id`,
 1 AS `package_id`,
 1 AS `user_id`,
 1 AS `type`,
 1 AS `actionstorage_id`,
 1 AS `operation_date`,
 1 AS `type_id`*/;
SET character_set_client = @saved_cs_client;

--
-- Temporary view structure for view `pkq_operations_withstorages`
--

DROP TABLE IF EXISTS `pkq_operations_withstorages`;
/*!50001 DROP VIEW IF EXISTS `pkq_operations_withstorages`*/;
SET @saved_cs_client     = @@character_set_client;
/*!50503 SET character_set_client = utf8mb4 */;
/*!50001 CREATE VIEW `pkq_operations_withstorages` AS SELECT 
 1 AS `operation_id`,
 1 AS `package_id`,
 1 AS `user_id`,
 1 AS `type`,
 1 AS `actionstorage_id`,
 1 AS `operation_date`,
 1 AS `type_id`,
 1 AS `commandingstorage_id`*/;
SET character_set_client = @saved_cs_client;

--
-- Table structure for table `storages`
--

DROP TABLE IF EXISTS `storages`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `storages` (
  `storage_id` int NOT NULL,
  `storage_addr` varchar(90) NOT NULL,
  PRIMARY KEY (`storage_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `storages`
--

LOCK TABLES `storages` WRITE;
/*!40000 ALTER TABLE `storages` DISABLE KEYS */;
INSERT INTO `storages` VALUES (0,'АдминОффис'),(1,'Крупской 12'),(2,'Дружная 42'),(3,'Складская 98/2');
/*!40000 ALTER TABLE `storages` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `unitof_weight`
--

DROP TABLE IF EXISTS `unitof_weight`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `unitof_weight` (
  `unitof_weight_id` int NOT NULL,
  `unitof_weight_title` varchar(45) NOT NULL,
  PRIMARY KEY (`unitof_weight_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `unitof_weight`
--

LOCK TABLES `unitof_weight` WRITE;
/*!40000 ALTER TABLE `unitof_weight` DISABLE KEYS */;
INSERT INTO `unitof_weight` VALUES (0,'Грамм'),(1,'Килограмм');
/*!40000 ALTER TABLE `unitof_weight` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `user_roles`
--

DROP TABLE IF EXISTS `user_roles`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `user_roles` (
  `role_id` int NOT NULL,
  `role` varchar(45) NOT NULL,
  PRIMARY KEY (`role_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `user_roles`
--

LOCK TABLES `user_roles` WRITE;
/*!40000 ALTER TABLE `user_roles` DISABLE KEYS */;
INSERT INTO `user_roles` VALUES (1,'manager'),(2,'storekeeper');
/*!40000 ALTER TABLE `user_roles` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `users`
--

DROP TABLE IF EXISTS `users`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `users` (
  `user_id` int NOT NULL,
  `storage_id` int NOT NULL,
  `role_id` int NOT NULL,
  `login` varchar(20) NOT NULL,
  `password` varchar(20) NOT NULL,
  `first_name` varchar(45) NOT NULL,
  `last_name` varchar(45) NOT NULL,
  `phone_num` varchar(12) NOT NULL,
  PRIMARY KEY (`user_id`),
  KEY `fk1_idx` (`role_id`),
  KEY `fk2_idx` (`storage_id`),
  CONSTRAINT `fk4` FOREIGN KEY (`role_id`) REFERENCES `user_roles` (`role_id`),
  CONSTRAINT `fk5` FOREIGN KEY (`storage_id`) REFERENCES `storages` (`storage_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `users`
--

LOCK TABLES `users` WRITE;
/*!40000 ALTER TABLE `users` DISABLE KEYS */;
INSERT INTO `users` VALUES (1,1,2,'log','pas','Артем','Юсупов','89373699112'),(2,2,2,'log2','pas','Иван','Иванов','89373699112'),(3,0,1,'add','add','Генадий','Админов','89373699112'),(4,3,2,'log3','pas','Богдан','Трунов','89373699113');
/*!40000 ALTER TABLE `users` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Temporary view structure for view `users_withroles`
--

DROP TABLE IF EXISTS `users_withroles`;
/*!50001 DROP VIEW IF EXISTS `users_withroles`*/;
SET @saved_cs_client     = @@character_set_client;
/*!50503 SET character_set_client = utf8mb4 */;
/*!50001 CREATE VIEW `users_withroles` AS SELECT 
 1 AS `user_id`,
 1 AS `storage_id`,
 1 AS `role_id`,
 1 AS `role`,
 1 AS `login`,
 1 AS `password`,
 1 AS `first_name`,
 1 AS `last_name`,
 1 AS `phone_num`*/;
SET character_set_client = @saved_cs_client;

--
-- Final view structure for view `packages_withstatus`
--

/*!50001 DROP VIEW IF EXISTS `packages_withstatus`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8mb4 */;
/*!50001 SET character_set_results     = utf8mb4 */;
/*!50001 SET collation_connection      = utf8mb4_0900_ai_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `packages_withstatus` AS select `operation_withnumber`.`package_id` AS `package_id`,`operation_withnumber`.`weight` AS `weight`,`operation_withnumber`.`weight_unit` AS `weight_unit`,`operation_withnumber`.`sender_Fname` AS `sender_Fname`,`operation_withnumber`.`sender_Sname` AS `sender_Sname`,`operation_withnumber`.`sender_Lname` AS `sender_Lname`,`operation_withnumber`.`sender_mail` AS `sender_mail`,`operation_withnumber`.`sender_number` AS `sender_number`,`operation_withnumber`.`recipient_Fname` AS `recipient_Fname`,`operation_withnumber`.`recipient_Sname` AS `recipient_Sname`,`operation_withnumber`.`recipient_Lname` AS `recipient_Lname`,`operation_withnumber`.`recipient_mail` AS `recipient_mail`,`operation_withnumber`.`recipient_number` AS `recipient_number`,`operation_withnumber`.`dimension_title` AS `dimension_title`,`operation_withnumber`.`status` AS `status`,`operation_withnumber`.`status_date` AS `status_date`,`operation_withnumber`.`actionstorage_id` AS `actionstorage_id` from (select `packages`.`package_id` AS `package_id`,`packages`.`weight` AS `weight`,`unitof_weight`.`unitof_weight_title` AS `weight_unit`,`packages`.`sender_Fname` AS `sender_Fname`,`packages`.`sender_Sname` AS `sender_Sname`,`packages`.`sender_Lname` AS `sender_Lname`,`packages`.`sender_mail` AS `sender_mail`,`packages`.`sender_number` AS `sender_number`,`packages`.`recipient_Fname` AS `recipient_Fname`,`packages`.`recipient_Sname` AS `recipient_Sname`,`packages`.`recipient_Lname` AS `recipient_Lname`,`packages`.`recipient_mail` AS `recipient_mail`,`packages`.`recipient_number` AS `recipient_number`,`dimensions`.`dimension_title` AS `dimension_title`,`pkg_operations_withtype`.`type` AS `status`,`pkg_operations_withtype`.`actionstorage_id` AS `actionstorage_id`,`pkg_operations_withtype`.`operation_date` AS `status_date`,row_number() OVER (PARTITION BY `packages`.`package_id` ORDER BY `pkg_operations_withtype`.`operation_date` desc )  AS `numb` from (((`packages` join `pkg_operations_withtype` on((`pkg_operations_withtype`.`package_id` = `packages`.`package_id`))) left join `dimensions` on((`packages`.`dimension_id` = `dimensions`.`dimension_id`))) left join `unitof_weight` on((`packages`.`unitof_weight_id` = `unitof_weight`.`unitof_weight_id`)))) `operation_withnumber` where (`operation_withnumber`.`numb` = 1) */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `pkg_operations_withtype`
--

/*!50001 DROP VIEW IF EXISTS `pkg_operations_withtype`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8mb4 */;
/*!50001 SET character_set_results     = utf8mb4 */;
/*!50001 SET collation_connection      = utf8mb4_0900_ai_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `pkg_operations_withtype` AS select `pkg_operations`.`operation_id` AS `operation_id`,`pkg_operations`.`package_id` AS `package_id`,`pkg_operations`.`user_id` AS `user_id`,`operation_types`.`type` AS `type`,`pkg_operations`.`actionstorage_id` AS `actionstorage_id`,`pkg_operations`.`operation_date` AS `operation_date`,`operation_types`.`type_id` AS `type_id` from (`pkg_operations` join `operation_types` on((`pkg_operations`.`type_id` = `operation_types`.`type_id`))) */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `pkq_operations_withstorages`
--

/*!50001 DROP VIEW IF EXISTS `pkq_operations_withstorages`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8mb4 */;
/*!50001 SET character_set_results     = utf8mb4 */;
/*!50001 SET collation_connection      = utf8mb4_0900_ai_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `pkq_operations_withstorages` AS select `pkg_operations_withtype`.`operation_id` AS `operation_id`,`pkg_operations_withtype`.`package_id` AS `package_id`,`pkg_operations_withtype`.`user_id` AS `user_id`,`pkg_operations_withtype`.`type` AS `type`,`pkg_operations_withtype`.`actionstorage_id` AS `actionstorage_id`,`pkg_operations_withtype`.`operation_date` AS `operation_date`,`pkg_operations_withtype`.`type_id` AS `type_id`,`users`.`storage_id` AS `commandingstorage_id` from (`pkg_operations_withtype` left join `users` on((`users`.`user_id` = `pkg_operations_withtype`.`user_id`))) */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `users_withroles`
--

/*!50001 DROP VIEW IF EXISTS `users_withroles`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8mb4 */;
/*!50001 SET character_set_results     = utf8mb4 */;
/*!50001 SET collation_connection      = utf8mb4_0900_ai_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `users_withroles` AS select `users`.`user_id` AS `user_id`,`users`.`storage_id` AS `storage_id`,`users`.`role_id` AS `role_id`,`user_roles`.`role` AS `role`,`users`.`login` AS `login`,`users`.`password` AS `password`,`users`.`first_name` AS `first_name`,`users`.`last_name` AS `last_name`,`users`.`phone_num` AS `phone_num` from (`users` join `user_roles` on((`users`.`role_id` = `user_roles`.`role_id`))) */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-05-31  4:34:23
