package com.hz.ctrl;

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

public class MapResourceCtrl extends HttpServlet {

	private static final long serialVersionUID = -588868652944469238L;

	@Override
	protected void doGet(HttpServletRequest req, HttpServletResponse resp) throws ServletException, IOException {
		todo(req, resp);
	}

	@Override
	protected void doPost(HttpServletRequest req, HttpServletResponse resp) throws ServletException, IOException {
		todo(req, resp);
	}


	private void todo (HttpServletRequest req, HttpServletResponse resp) throws ServletException, IOException {
		String result = "";
		String method = req.getParameter("method");
		if (method != null) {
			switch(method) {
				case "getDirList": result = getDirList(); break;
				case "getDirTree": result = getDirTree(req); break;
				case "loadConfig": result = ConfigUtils.loadConfig(); break;
				case "isExistFile": result = isExistFile(req); break;
				default: result = "参数method的有效为: getDirList、getDirTree、loadConfig、isExistFile"; break;
			}
		} else {
			result = "缺少参数[method], 其有效值为：getDirList（获取模型根目录列表）、getDirTree（获取模型目录结构）、loadConfig（加载配置文件）、isExistFile（检查文件是否存在）";
		}

		resp.setHeader("Content-type", "text/html;charset=UTF-8");
		resp.setHeader("Access-Control-Allow-Origin", "*");
		resp.getWriter().write(result);
		resp.getWriter().flush();
		resp.getWriter().close();
	}


	/*
	 * 获取目录列表
	 */
	private String getDirList () {
		JSONArray jsonArr = new JSONArray();
		JSONObject jsonObj = null;

		String filePath = null;
		String rltvPath = "models";
		File file = null;

		try {
			filePath = this.getServletContext().getRealPath(rltvPath);
			file = new File(filePath);

			for(String name : file.list()) {
				jsonObj = new JSONObject();
				jsonObj.put("name", name);
				jsonObj.put("path", rltvPath + "/" + name);
				jsonArr.add(jsonObj);
			}
			return jsonArr.toJSONString();
		} catch (Exception e) {
			return "获取模型目录列表异常：" + e.getMessage();
		}
	}


	/*
	 * 获取目录结构树
	 */
	private String getDirTree (HttpServletRequest req) {

		String dirName = req.getParameter("dirName");	// 目录名称
		String filters = req.getParameter("filters");	// 过滤文件
		String rltvPath = "models";
		String filePath = null;
		File file = null;

		JSONArray filterList = new JSONArray();
		JSONArray jsonList = new JSONArray();
		JSONObject jsonObj = null;
		JSONArray list = null;
		BuildingUtils utils = null;

		try {

			if (dirName == null || dirName.length() == 0) {
				return "1.缺少参数[dirName]：目录名称（必填）；2.缺少参数[filters]：过滤文件名称，多个用逗号分隔（可选）；";
			}

			// 解析文件
			rltvPath += dirName.length() > 0 ? ("/" + dirName) : "";
			filePath = this.getServletContext().getRealPath(rltvPath);
			file = new File(filePath);
			utils = new BuildingUtils(filePath);


			// 解析过滤的文件名称
			if (filters != null && filters.length() > 0) {
				for(String val : filters.split(",")) {
					filterList.add(val);
				}
			}

			jsonObj = new JSONObject();
			jsonObj.put("remark", utils.getConfig(dirName, dirName));
			jsonObj.put("name", dirName);
			jsonObj.put("path", rltvPath);
			jsonList.add(jsonObj);

			if (file.isDirectory()) {
				jsonObj.put("list", list = new JSONArray());
				iterateDirectory(list, filterList, rltvPath, file, utils);
			}			

			return jsonList.toJSONString();
		} catch (Exception e) {
			return "获取模型目录结构异常：" + e.getMessage();
		}
	}


	/**
	 * 迭代目录结构
	 * @param children		存放子集的对象
	 * @param filterList	过滤的名称集合
	 * @param rltvPath		父级相对路径
	 * @param parentFile	父级文件对象
	 * @param level			目录等级	
	 * @throws Exception
	 */
	private void iterateDirectory(JSONArray children, JSONArray filterList, String rltvPath, File parentFile, BuildingUtils utils) throws Exception {
		JSONObject jsonObj = null;
		JSONArray list = null;
		File file = null;

		for(String fileName : parentFile.list()) {
			if (filterList.contains(fileName)) continue;

			file = new File(parentFile.getPath() + "\\" + fileName);
			jsonObj = new JSONObject();
			jsonObj.put("remark", utils.getConfig(fileName, fileName));
			jsonObj.put("name", fileName);
			jsonObj.put("path", rltvPath + "/" + fileName);
			children.add(jsonObj);

			if (file.isDirectory()) {
				jsonObj.put("list", list = new JSONArray());
				iterateDirectory(list, filterList, rltvPath + "/" + fileName, file, utils);
			}
		}
	}


	/**
	 * 检查文件是否存在
	 * @param req
	 */
	private String isExistFile (HttpServletRequest req) {
		JSONObject jsonObj = new JSONObject();
		String filePath = req.getParameter("path");
		File file = null;

		try {

			if (filePath != null && filePath.length() > 0) {
				filePath = this.getServletContext().getRealPath(filePath);
				file = new File(filePath);
				jsonObj.put("exists", file != null && file.exists());
			} else {
				jsonObj.put("exists", false);
				jsonObj.put("remark", "缺少参数[path]：文件或文件夹的相对路径");
			}

		} catch (Exception e) {
			jsonObj.put("exists", false);
			jsonObj.put("remark", "检查文件是否存在时异常：" + e.getMessage());
		}
		return jsonObj.toString();
	}
}
