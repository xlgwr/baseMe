# 正式环境打包部署

#* Nodejs Serve 方式
* 安装，打包，运行
```
npm install -g serve

npm run build

serve -s dist -l 8088
```

#* docker nginx 方式
* nginx 配置
```
server {
    listen 80;
    listen [::]:80;
    # 接口服务的IP地址
    server_name localhost;
    charset utf-8;
    access_log off;
    # ElecManageSystem-应用文件夹名称 app-index.html页面所在文件夹
    root /var/www/html;
    index index.html;

    location / {
        try_files $uri $uri/ /index.html;
    }

    location /getXXX{
        proxy_pass http://abc.com/d/key/getXXX;
    }
    location /getAAA {
        proxy_pass http://abc.com/d/key/getAAA;
    }
    location /getBBB{
        proxy_pass http://abc.com/d/key/getBBB;
    }
    error_page 500 502 503 504 /50x.html;
    location = /50x.html {
        root html;
    }
}
```
* docker 文件
```
FROM nginx
COPY ./dist /var/www/html
COPY ./nginx/custom.conf /etc/nginx/conf.d/ 
RUN rm /etc/nginx/conf.d/default.conf
EXPOSE 80
CMD ["nginx","-g","daemon off;"] 
```
# 生成images
```
docker build -t fg-vue .
```

# 启动容器：
```
docker run -it --name fg-vue1 --restart=always -p 8088:80 -d fg-vue 
```

