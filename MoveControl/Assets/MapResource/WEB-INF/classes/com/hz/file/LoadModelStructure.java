package com.hz.file;

import java.io.File;
import java.io.IOException;

import javax.servlet.ServletException;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

import com.alibaba.fastjson.JSONArray;
import com.alibaba.fastjson.JSONObject;
import com.hz.utils.BuildingUtils;
import com.hz.utils.ConfigUtils;

/**
 * 加载地图模型文件目录结构
 * @author xieyh
 */
public class LoadModelStructure extends HttpServlet {

	private static final long serialVersionUID = -3480423193151421503L;

	private JSONArray filter = new JSONArray();
	private JSONArray array = new JSONArray();
	private BuildingUtils bu = null;
	private int fileId = 1001;

	private static final String TYPE_DIRE = "1";
	private static final String TYPE_FILE = "2";

	private static final String FILE_FLAG_0 = "0";
	private static final String FILE_FLAG_1 = "1";
	private static final String FILE_FLAG_2 = "2";
	private static final String FILE_FLAG_3 = "3";
	private static final String FILE_FLAG_4 = "4";
	private static final String FILE_FLAG_5 = "5";
	private static final String FILE_FLAG_6 = "6";



	@Override
	protected void doGet(HttpServletRequest req, HttpServletResponse resp) throws ServletException, IOException {
		todo(req, resp);
	}


	@Override
	protected void doPost(HttpServletRequest req, HttpServletResponse resp) throws ServletException, IOException {
		todo(req, resp);
	}


	/*
	 * 请求处理
	 */
	private void todo (HttpServletRequest req, HttpServletResponse resp) throws ServletException, IOException {
		String name = req.getParameter("path");
		String filter = req.getParameter("filter");

		String path = null;
		String realPath = null;
		File file = null;
		
		int fileId = 0;

		array = new JSONArray();

		if (name != null && name.length() > 0) {
			path = "models/" + name;

			initFilter(filter);
			readConfig(path);

			realPath = this.getServletContext().getRealPath(path);
			file = new File(realPath);
			fileId = setData(0, path, name, TYPE_DIRE, FILE_FLAG_0, 1, 1);

			// 根目录文件结构数据
			setRoots(fileId, file, path + "/");
		}


		// 设置响应参数
		resp.setHeader("Content-type", "text/html;charset=UTF-8");
		resp.setHeader("Access-Control-Allow-Origin","*");
		resp.getWriter().write(array.toJSONString());
		resp.getWriter().flush();
		resp.getWriter().close();

		array = null;
	}


	/**
	 * 设置根目录文件结构数据
	 * @param parentId
	 * @param parentFile
	 * @param parentPath
	 */
	private void setRoots (int parentId, File parentFile, String parentPath) {
		// 读取根目录下的文件结构（只读取文件夹）
		if (parentFile != null && parentFile.isDirectory()) {

			String fileName = null;
			String filePath = null;
			String fileFlag = null;

			int fileId = 0;
			int order = 1;

			for(File file : parentFile.listFiles()) {
				fileName = file.getName();

				if (file.isDirectory() && !filter.contains(fileName)) {
					filePath = parentPath + fileName;
					fileId = setData(parentId, filePath, fileName, TYPE_DIRE, FILE_FLAG_0, 2, order++);

					// 获取配置文件中属于存放建筑模型文件的文件夹名称，并以此来判断下级文件的文件标识
					if (fileName.equals(ConfigUtils.getConfig("MODEL.BUILDING.FILENAME"))) {
						fileFlag = FILE_FLAG_1;
					} else {
						fileFlag = FILE_FLAG_0;
					}

					setChildNodes(fileId, file, filePath + "/", fileFlag, 3);
				}
			}
		}
	}



	/**
	 * 设置子集节点数据
	 * @param parentId
	 * @param parentPath
	 * @param parentFile
	 * @param childFlag
	 * @param childLevel
	 */
	private void setChildNodes(int parentId, File parentFile, String parentPath, String childFlag, int childLevel) {
		String fileName = null;
		String filePath = null;
		String fileFlag = null;

		int fileId = 0;
		int index = 0;
		int order = 1;

		try {

			for(File file : parentFile.listFiles()) {
				fileName = file.getName();
				filePath = parentPath + fileName;
				fileFlag = childFlag;

				if (file.isDirectory()) {

					// 如果文件标识属于楼层内，则要判断文件名来重新定义标识
					if (fileFlag.equals(FILE_FLAG_3) && fileName.equals("out")) {
						fileFlag = FILE_FLAG_4;
					}

					// 设置文件结构数据并获取数据ID提供给下面的子目录使用
					fileId = setData(parentId, filePath, fileName, TYPE_DIRE, fileFlag, childLevel, order++);

					// 如果此目录文件标识属于楼栋，则下级目录的文件标识为楼层
					switch(fileFlag) {
						case FILE_FLAG_1: fileFlag = FILE_FLAG_2; break;
						case FILE_FLAG_2: fileFlag = FILE_FLAG_3; break;
					}

					// 读取并设置子目录结构数据
					setChildNodes(fileId, file, filePath + "/", fileFlag, childLevel + 1);

				} else {

					// 获取最后一个.的位置，用于分割文件名和后缀名
					index = fileName.lastIndexOf(".");

					// 获取并检查文件后缀名是否再规则之内
					if (ConfigUtils.checkSuffix(fileName.toLowerCase().substring(index + 1))) {

						// 获取文件的文件标识（统一转换成小写去比较）
						fileFlag = getFileFlag(fileName.toLowerCase().substring(0, index).split("_"));

						// 设置文件结构数据
						setData(parentId, filePath, fileName, TYPE_FILE, fileFlag, childLevel, order++);
					}
				}
			}
		} catch (Exception e) {
			System.out.println();
		}
	}


	/**
	 * 
	 * @param parentData
	 * @param filePath
	 * @param fileName
	 * @param fileType
	 * @param fileFlag
	 * @param fileLevel
	 * @param fileOrder
	 */
	private int setData (int pid, String filePath, String fileName, String fileType, String fileFlag, int fileLevel, int fileOrder) {
		JSONObject jsonObj = new JSONObject();

		jsonObj.put("title", getConfig(fileName));	// 文件标题
		jsonObj.put("id", fileId);			// 文件编号

		jsonObj.put("pid", pid);			// 文件父编号，0表示根目录文件
		jsonObj.put("name", fileName);		// 文件名称
		jsonObj.put("path", filePath);		// 相对路径
		jsonObj.put("type", fileType);		// 文件类型：1.文件夹、2.文件
		jsonObj.put("flag", fileFlag);		// 文件标识：0其它、1楼栋、2楼层、3楼层内、4楼层外、5内模型文件、6外模型文件
		jsonObj.put("level", fileLevel);	// 结构等级
		jsonObj.put("order", fileOrder);	// 文件排序
		array.add(jsonObj);

		return fileId++;
	}



	/*
	 * 读取配置文件
	 */
	private void readConfig (String path) {
		bu = new BuildingUtils();
		bu.readConfig(this.getServletContext().getRealPath(path));
	}


	/*
	 * 获取配置
	 */
	private String getConfig (String key) {
		return bu.getConfig(key, key);
	}


	/*
	 * 获取文件标识
	 */
	private String getFileFlag (String[] strs) {
		if (strs[strs.length-1].equals("t005")) {
			if (strs[strs.length-2].equals("out")) return FILE_FLAG_6;
			if (strs[strs.length-2].equals("in")) return FILE_FLAG_5;
		} else {
			if (strs[strs.length-1].equals("out")) return FILE_FLAG_6;
			if (strs[strs.length-1].equals("in")) return FILE_FLAG_5;
		}
		return FILE_FLAG_0;
	}


	/*
	 * 初始化过滤对象
	 */
	private void  initFilter (String value) {
		// 加载过滤配置
		String[] values = null;
		if (value == null || value.length() == 0) {
			value = ConfigUtils.getConfig("LOAD.FILE.FILTER");
		}

		if (value.length() > 0) {
			values = value.split(",");
			filter.clear();

			for(int i = 0, len = values.length; i < len; i++) {
				filter.add(values[i]);
			}
		}
	}

}
