package com.hz.file;

import java.io.File;
import java.io.IOException;

import javax.servlet.ServletException;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

import com.alibaba.fastjson.JSONArray;
import com.alibaba.fastjson.JSONObject;
import com.hz.utils.ConfigUtils;


/**
 * 获取模型文件资源列表
 * @author xie.yh
 */
public class LoadMapFile extends HttpServlet {
	private static final long serialVersionUID = 4074596949854249869L;

	private JSONArray array = new JSONArray();		// 返回结果集
	private static final String basePath = "models/";

	public LoadMapFile () {

	}

	@Override
	protected void doGet(HttpServletRequest req, HttpServletResponse resp) throws ServletException, IOException {
		todo(req, resp);
	}
	
	@Override
	protected void doPost(HttpServletRequest req, HttpServletResponse resp) throws ServletException, IOException {
		todo(req, resp);
	}


	private void todo (HttpServletRequest req, HttpServletResponse resp) throws ServletException, IOException {
		String name = req.getParameter("name");
		String path = basePath; // "models" + (name.length() > 0 ? ("/" + name) : "");
		String realPath = null;
		File file = null;

		array = new JSONArray();

		if (name != null && name.length() > 0) {
			path += name;
			realPath = this.getServletContext().getRealPath(path);
			file = new File(realPath);

			if (file != null && file.isDirectory()) {
				forEachFile(path + "/", file.list());
			}			
		}

		resp.setHeader("Content-type", "text/html;charset=UTF-8");
		resp.setHeader("Access-Control-Allow-Origin", "*");
		resp.getWriter().write(array.toJSONString());
		resp.getWriter().flush();
		resp.getWriter().close();
	}




	/**
	 * 循环读取文件
	 * @param filePath	文件路径，结尾到/符号
	 * @param fileNames	文件列表
	 */
	private void forEachFile(String filePath, String[] fileNames) {
		String realPath = null;		// 真实路径
		File fileObj = null;		// 文件对象
		JSONObject jsonObj = null;	// 
		int index = 0;

		try {
			// 获取真实路径
			realPath = this.getServletContext().getRealPath(filePath) + "\\";

			// 循环读取文件列表
			for(String fileName : fileNames) {
				fileObj = new File(realPath + fileName);

				// 判断是文件还是文件夹
				if (fileObj.isFile()) {
					index = fileName.lastIndexOf(".");

					// 检查文件后缀，只读取指定后缀的文件
					if (ConfigUtils.checkSuffix(fileName.substring(index + 1))) {
						jsonObj = new JSONObject();
						jsonObj.put("name", fileName.substring(0, index));	// 文件名称（不带后缀）
						jsonObj.put("path", filePath);	// 相对路径

						// 新属性
						jsonObj.put("fileName", fileName);	// 文件（带后缀）
						array.add(jsonObj);
					}
				} else {
					forEachFile(filePath + fileName + "/", fileObj.list());
				}
			}
		} catch (Exception e) {
			System.out.println("循环读取文件时异常：" + e.getMessage());
		}
	}
}
