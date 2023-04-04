CREATE TABLE IF NOT EXISTS `nguoidung` (
  `TenDangNhap` varchar(100) COLLATE utf8_vietnamese_ci NOT NULL,
  `MatKhau` varchar(100) COLLATE utf8_vietnamese_ci NOT NULL,
  `HoTen` varchar(100) COLLATE utf8_vietnamese_ci DEFAULT NULL,
  `Quyen` varchar(10) COLLATE utf8_vietnamese_ci NOT NULL,
  `MaBoPhan` varchar(50) COLLATE utf8_vietnamese_ci DEFAULT NULL,
  CONSTRAINT PRIMARY KEY(`TenDangNhap`),
  CONSTRAINT `FK_ND_BP` FOREIGN KEY (`MaBoPhan`) REFERENCES `bophan`(`MaBoPhan`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_vietnamese_ci;
