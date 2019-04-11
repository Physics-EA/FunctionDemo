package com.hz.utils;

import java.io.BufferedInputStream;
import java.io.FileInputStream;
import java.io.FileNotFoundException;
import java.io.IOException;
import java.util.PropertyResourceBundle;
import java.util.ResourceBundle;

public class BuildingUtils {

	private ResourceBundle sysConfig = null;	// 系统配置文件
	private ResourceBundle outConfig = null;	// 外部配置文件

	public BuildingUtils() {

	}

	public BuildingUtils(String realPath) {
		this.readConfig(realPath);
	}

	/**
	 * 读取模型的说明书文件
	 * @param realPath 文件路径
	 */
	public void readConfig (String realPath) {
		String specification = ConfigUtils.getConfig("MODEL.SPECIFICATION");
		String filePath = realPath + "/" + specification;
		BufferedInputStream in = null;

		// 获取系统默认的模型文件说明书
		sysConfig = ResourceBundle.getBundle("building");

		// 获取模型文件内的模型文件说明书
        try {
            in = new BufferedInputStream(new FileInputStream(filePath));  
            outConfig = new PropertyResourceBundle(in);
        } catch (FileNotFoundException e) {
        	System.out.println("提示：模型文件根目录下没有配置" + specification + "模型说明文件，系统使用默认的说明文件!");
        } catch (IOException e) {
        	System.out.println("提示：读取解析" + specification + "模型说明文件失败，系统使用默认的说明文件!");
		} finally {
			try {
				if (in != null) in.close();
			} catch (IOException e) {
				System.out.println("关闭文件错误" + e.getMessage());
			}
		}
	}


	/**
	 * 获取建筑模型的配置
	 * @param key 模块文件夹名称
	 * @param def 默认名称
	 * @return
	 */
	public String getConfig (String key, String def) {
		String val = null;
		try {

			// 优先获取外部配置文件
			if (outConfig != null && outConfig.containsKey(key)) {
				val = outConfig.getString(key);
				val = new String(val.getBytes("iso8859-1"), "utf8");
			}

			// 没有再获取系统配置文件
			if (val == null || val.length() == 0) {
				if (sysConfig != null && sysConfig.containsKey(key)) {
					val = sysConfig.getString(key);
					val = new String(val.getBytes("iso8859-1"), "utf8");
				}
			}

			return val == null || val.length() == 0 ? def : val;
		} catch (Exception e) {
			System.out.println("获取建筑模型的配置失败：" + e.getMessage());
			return def;
		}
	}
}
