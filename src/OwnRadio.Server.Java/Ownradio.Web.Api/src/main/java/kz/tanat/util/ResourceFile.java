package kz.tanat.util;

import org.springframework.util.ResourceUtils;
import org.springframework.web.multipart.MultipartFile;

import java.io.File;
import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.Paths;

public class ResourceFile {
    public static final String UPLOADING_DIR = System.getProperty("user.dir") + "/userfile/";

    private ResourceFile() {
    }

    public static byte[] read(String fileName) {
        File file;
        try {
            file = ResourceUtils.getFile(fileName);
            return Files.readAllBytes(file.toPath());
        } catch (IOException e) {
            throw new RuntimeException("Error read resource");
        }
    }

    public static String save(String userDir, String fileName, MultipartFile file) {
        try {
            File dir = new File(UPLOADING_DIR + userDir);
            if (!dir.exists()) {
                dir.mkdirs();
            }

            String filePath = Paths.get(UPLOADING_DIR, userDir, fileName).toString();
            file.transferTo(new File(filePath));

            return filePath;
        } catch (Exception e) {
            throw new RuntimeException("Error save file", e);
        }
    }

}
