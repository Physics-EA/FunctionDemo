package com.hz.utils;

import java.util.ResourceBundle;

import com.alibaba.fastjson.JSON;
import com.alibaba.fastjson.JSONArray;

public class ConfigUtils {

	private static ResourceBundle bundle = null;
	private static JSONArray suffixs = null;	// 后缀过滤


	/*
	 * 加载配置文件
	 */
	public static synchronized String loadConfig () {
		bundle = ResourceBundle.getBundle("config");

		suffixsInit();
		
		return "加载成功";
	}

	/*
	 * 获取配置
	 */
	public static synchronized String getConfig (String key) {		
		String value = "";

		if (bundle == null) {
			loadConfig();
		}

		if (bundle != null) {
			value = bundle.getString(key);
		}

		if (value == null) {
			value = "";
		}

		return value;
	}


	/**
	 * 初始化后缀配置（先读取配置文件，如果有异常再使用默认的）
	 */
	private static void suffixsInit () {
		try {
			suffixs = JSON.parseArray(getConfig("MODEL.FILE.SUFFIXS"));
		} catch (Exception e) {
			suffixs = JSON.parseArray("['obj','unity','assetbundle']");
		}
	}


	/**
	 * 检查文件后缀是否属于规定范围之内
	 * @param suffix
	 * @return
	 */
	public static synchronized boolean checkSuffix (String suffix) {
		if (suffixs == null) {
			suffixsInit();
		}
		return suffixs.contains(suffix);
	}
}
