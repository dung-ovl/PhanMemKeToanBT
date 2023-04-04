CREATE TRIGGER after_ctpx_update 
    AFTER UPDATE ON ct_phieuxuat
    FOR EACH ROW 
 UPDATE phieuxuat
 SET TongTien = TongTien - OLD.ThanhTien + NEW.ThanhTien
 WHERE SoPhieu = NEW.SoPhieu 
   
