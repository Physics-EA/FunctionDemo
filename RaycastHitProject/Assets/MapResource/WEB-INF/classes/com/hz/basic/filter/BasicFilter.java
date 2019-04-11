package com.hz.basic.filter;

import java.io.IOException;

import javax.servlet.Filter;
import javax.servlet.FilterChain;
import javax.servlet.FilterConfig;
import javax.servlet.ServletException;
import javax.servlet.ServletRequest;
import javax.servlet.ServletResponse;
import javax.servlet.http.HttpServletResponse;

public class BasicFilter implements Filter{

//	private static String[] strs = null;

	@Override
	public void destroy() {
		// TODO Auto-generated method stub
	}

	@Override
	public void doFilter(ServletRequest arg0, ServletResponse arg1, FilterChain arg2) throws IOException, ServletException {
		HttpServletResponse response = (HttpServletResponse) arg1;

		/*
		ResourceBundle bundle = ResourceBundle.getBundle("config");
		String config = bundle.getString("ACCESS.CONTROL.ALLOW.ORIGIN");
		if (config != null) {
			strs = config.split(",");
		}

		String str = null;
		String host = arg0.getRemoteHost();

		if (strs != null) {
			
			for(int i = 0, I = strs.length; i < I; i++) {
				str = strs[i];

				if (str != null && !str.isEmpty()) {
					if (str.indexOf(host) > -1 || host.indexOf(str) > -1) {
						// 指定允许其他域名访问  
						//response.addHeader("Access-Control-Allow-Origin", str);
					}
				}
			}
		}
		 */

		response.addHeader("Access-Control-Allow-Origin", "*");

		// 响应类型  响应方法
	    response.setHeader("Access-Control-Allow-Headers", "Content-Type");  
		response.setHeader("Access-Control-Allow-Methods", "GET,POST,OPTIONS");  
		arg2.doFilter(arg0, response);
	}

	@Override
	public void init(FilterConfig arg0) throws ServletException {
		// TODO Auto-generated method stub
	}
}
