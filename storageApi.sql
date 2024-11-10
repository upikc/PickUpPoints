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
  `client_fullname` varchar(150) NOT NULL,
  `client_mail` varchar(90) NOT NULL,
  `client_number` varchar(12) NOT NULL,
  PRIMARY KEY (`package_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `packages`
--

LOCK TABLES `packages` WRITE;
/*!40000 ALTER TABLE `packages` DISABLE KEYS */;
INSERT INTO `packages` VALUES (0,1.00,'Иванов Иван Иванович','uupikc@gmail.com','89373699112'),(1,1.00,'Иванов Иван Иванович','uupikc@gmail.com','89373699112'),(2,0.50,'Иванов Иван Иванович','uupikc@gmail.com','89373699112'),(3,0.20,'Иванов Иван Иванович','uupikc@gmail.com','89373699112'),(4,2.00,'Иванов Иван Иванович','uupikc@gmail.com','89373699112'),(5,0.70,'Иванов Иван Иванович','uupikc@gmail.com','89373699112'),(6,2.40,'Иванов Иван Иванович','uupikc@gmail.com','89373699112'),(7,1.30,'Иванов Иван Иванович','uupikc@gmail.com','89373699112'),(8,3.90,'Иванов Иван Иванович','uupikc@gmail.com','89373699112'),(9,4.00,'Иванов Иван Иванович','uupikc@gmail.com','89373699112'),(10,3.20,'Иванов Иван Иванович','uupikc@gmail.com','89373699112');
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
 1 AS `client_fullname`,
 1 AS `client_mail`,
 1 AS `client_number`,
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
) ENGINE=InnoDB AUTO_INCREMENT=22 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `pkg_operations`
--

LOCK TABLES `pkg_operations` WRITE;
/*!40000 ALTER TABLE `pkg_operations` DISABLE KEYS */;
INSERT INTO `pkg_operations` VALUES (1,0,1,0,'2024-09-15 12:20:00',1),(2,1,2,0,'2024-09-15 12:25:00',1),(3,2,5,0,'2024-09-15 12:20:00',2),(4,3,3,0,'2024-09-15 12:20:00',1),(5,4,6,0,'2024-09-15 12:20:00',2),(6,5,1,0,'2024-09-15 12:25:00',1),(7,6,1,0,'2024-09-15 12:30:00',1),(8,7,6,0,'2024-09-15 12:20:00',2),(9,8,9,0,'2024-09-15 12:25:00',3),(10,9,2,0,'2024-09-15 12:25:00',1),(11,10,10,0,'2024-09-15 12:20:00',3),(12,6,1,3,'2024-09-18 11:20:00',1),(13,8,9,3,'2024-09-19 13:40:00',3),(14,2,5,1,'2024-09-17 12:30:00',1),(16,7,9,1,'2024-09-15 13:50:00',3),(17,9,2,1,'2024-09-17 09:20:00',3),(18,9,9,2,'2024-09-17 11:20:00',3),(19,2,2,2,'2024-09-18 12:40:00',1);
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
INSERT INTO `storages` VALUES (0,'Фантомный СКлад'),(1,'Первая улица 48/1'),(2,'Вторая улица 28/2'),(3,'Третья улица 14/3');
/*!40000 ALTER TABLE `storages` ENABLE KEYS */;
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
INSERT INTO `users` VALUES (0,0,1,'qwer','qwer','qwer','qwer','qwer'),(1,1,1,'qwerty','123','Иван','Иванов','89373688112'),(2,1,2,'qwert','123','Дмитрий','Нагиев','89373228228'),(3,1,2,'qwerti','123','Илья','Костин','89362885427'),(4,2,1,'dataAdmin','123','Сима','Воронин','81611115116'),(5,2,2,'dataB','123','Марк','Симонов','83204675874'),(6,2,2,'dataC','123','Виктор','Филимонов','852790597718'),(7,2,2,'dataD','123','Алексей','Юдин','834944835772'),(8,3,1,'user1','123','Матвей','Крылов','83128706786'),(9,3,2,'user2','123','Никита','Зыков','8806958799'),(10,3,2,'user3','123','Кирилл','Нечаев','818098605798');
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
-- Dumping events for database 'dbstorage'
--

--
-- Dumping routines for database 'dbstorage'
--

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
/*!50001 VIEW `packages_withstatus` AS select `operation_withnumber`.`package_id` AS `package_id`,`operation_withnumber`.`weight` AS `weight`,`operation_withnumber`.`client_fullname` AS `client_fullname`,`operation_withnumber`.`client_mail` AS `client_mail`,`operation_withnumber`.`client_number` AS `client_number`,`operation_withnumber`.`type` AS `status`,`operation_withnumber`.`operation_date` AS `status_date`,`operation_withnumber`.`actionstorage_id` AS `actionstorage_id` from (select `packages`.`package_id` AS `package_id`,`packages`.`weight` AS `weight`,`packages`.`client_fullname` AS `client_fullname`,`packages`.`client_mail` AS `client_mail`,`packages`.`client_number` AS `client_number`,`pkg_operations_withtype`.`type` AS `type`,`pkg_operations_withtype`.`actionstorage_id` AS `actionstorage_id`,`pkg_operations_withtype`.`operation_date` AS `operation_date`,row_number() OVER (PARTITION BY `packages`.`package_id` ORDER BY `pkg_operations_withtype`.`operation_date` desc )  AS `numb` from (`packages` join `pkg_operations_withtype` on((`pkg_operations_withtype`.`package_id` = `packages`.`package_id`)))) `operation_withnumber` where (`operation_withnumber`.`numb` = 1) */;
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

-- Dump completed on 2024-11-11  4:19:02
