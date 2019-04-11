package com.hz.file;

import java.io.File;
import java.io.IOException;

import javax.servlet.ServletException;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

import com.alibaba.fastjson.JSON;
import com.alibaba.fastjson.JSONArray;
import com.alibaba.fastjson.JSONObject;
import com.hz.utils.BuildingUtils;
import com.hz.utils.ConfigUtils;


/**
 * 加载地图模型文件目录结构
 *
 * @author chendm
 *
 * @date 2016年12月12日
 */
public class LoadMapModel extends HttpServlet {
	private static final long serialVersionUID = -7840612000276334008L;

	private JSONArray filter = new JSONArray();
	private JSONArray array = new JSONArray();
	private BuildingUtils bu = null;
	private int tid = 1;


	@Override
	protected void doGet(HttpServletRequest req, HttpServletResponse resp) throws ServletException, IOException {
		todo(req, resp);
	}

	@Override
	protected void doPost(HttpServletRequest req, HttpServletResponse resp) throws ServletException, IOException {
		todo(req, resp);
	}


	private void todo (HttpServletRequest req, HttpServletResponse resp) throws ServletException, IOException {
		String path = req.getParameter("path");
		String value = req.getParameter("filter");

		array = new JSONArray();

		if (path != null && path.length() > 0) {
			path = "models/" + path;

			initFilter(value);
			readConfig(path);
			execute(path, 0, 0, 0);
		}

		// 设置响应参数
		resp.setHeader("Content-type", "text/html;charset=UTF-8");
		resp.setHeader("Access-Control-Allow-Origin","*");
		resp.getWriter().write(array.toJSONString());
		resp.getWriter().flush();
		resp.getWriter().close();

		array = null;
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


	/*
	 * 读取配置文件
	 */
	private void readConfig (String path) {
		bu = new BuildingUtils();
		bu.readConfig(this.getServletContext().getRealPath(path));
	}

	private String getConfig (String key) {
		return bu.getConfig(key, key);
	}


	/*
	 * 获取模型标识
	 */
	private String getModelFlag (String[] strs) {
		if (strs[strs.length-1].equals("t005")) {
			if (strs[strs.length-2].equals("out")) return "6";
			if (strs[strs.length-2].equals("in")) return "5";
		} else {
			if (strs[strs.length-1].equals("out")) return "6";
			if (strs[strs.length-1].equals("in")) return "5";
		}
		return "0";
	}



	/**
	 * 加载模型文件
	 * @param path 文件全路径
	 * @param level	层次等级标识，如：第一层全是0，第二层全是1，以此类推...
	 * @param pid 父id
	 * @return JSONArray {
	 * 				path : 全路径,
	 * 			  isFile : 是否是文件，true（文件）  false（文件夹），
	 * 			   level : 层次等级标识，默认从0开始
	 * 				name : 文件名称（包含后缀名），
	 * 				  id : 默认id从1开始
	 * 				 pid : 父id
	 * 				flag : 文件类型：0其它、1楼栋、2楼层、3楼层内、4楼层外
	 * 			}
	 */
	private String execute(String path, int level, int pid, int order) {
		String[] filesName = null;
		String fileName = null;
		String filePath = path;
		String fileFlag = "0";
		String flag = "0";
		File directory = null;
		File file = null;

		JSONObject json = null;
		int id = tid++;

		// 存放解析文件名称后的字符串集合
		String [] strs = null;

		try {
			filePath = this.getServletContext().getRealPath(path);
			directory = new File(filePath);
			filesName = directory.list();
			fileName = directory.getName();

			JSONObject jsonObj = new JSONObject();
			jsonObj.put("path", path);
			jsonObj.put("type", "1");
			jsonObj.put("level", level++);
			jsonObj.put("name", fileName);
			jsonObj.put("title", getConfig(fileName));
			jsonObj.put("id", id);
			jsonObj.put("pid", pid);
			jsonObj.put("flag", fileFlag);
			jsonObj.put("order", order);
			array.add(jsonObj);

			if (filesName != null) {
				for(int i = 0; i < filesName.length; i++) {
					fileName = filesName[i];
					file = new File(filePath + "\\" + fileName);

					if (file.isFile()) {
						strs = fileName.toLowerCase().split("\\.");	// 统一转成小写之后再分解文件

						// 是否包含指定后缀
						if (ConfigUtils.checkSuffix(strs[strs.length - 1])) {
							// 解析文件标识
							fileFlag = flag = getModelFlag(strs[0].split("_"));

							json = new JSONObject();
							json.put("path", path + "/" + fileName); //file.getPath());
							json.put("type", "2");
							json.put("level", level);
							json.put("name", fileName);
							json.put("title", fileName);
							json.put("id", tid++);
							json.put("pid", id);
							json.put("flag", fileFlag);
							json.put("order", i);
							array.add(json);
						}
					} else if (file.isDirectory()) {
						// 过滤不需要的文件
						if(!filter.contains(fileName)) {
							flag = execute(path + "/" + fileName, level, id, i);
						}
					}
				}
			}

			switch(flag) {
				case "6": flag = "4"; break;	// 楼层内
				case "5": flag = "3"; break;	// 楼层外
				case "4": 
				case "3": flag = "2"; break;	// 楼层
				case "2": flag = "1"; break;	// 楼栋
				default: flag = "0"; break;		// 其它
			}

			jsonObj.put("flag", flag);

		} catch (Exception e) {
			e.printStackTrace();
		}
		return flag;
	}

	public static void main(String[] args) {
//		LoadMapModel l = new LoadMapModel();
//		l.execute("models", 0, -1, 0);
//		l.array.forEach(p ->{
//			System.out.println(p);
//		});
		
		JSONArray suffixs = JSON.parseArray("['obj','mtl']");
		System.out.println(suffixs.size());
	}
}
