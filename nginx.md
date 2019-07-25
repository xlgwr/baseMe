# nginx+docker同一服务器上部署多个docker实现负载均衡

## nginx配置文件

## 配置主机host文件 ip 127.0.0.1 对应 www.abc.com 
 
```
upstream localhost {
      server 127.0.0.1:8080 weight=1;
      server 127.0.0.1:8081 weight=2;
}
server {
    listen 80;
    listen [::]:80;
    # 接口服务的IP地址
    server_name localhost;
    charset utf-8;
    access_log off;
    # ElecManageSystem-应用文件夹名称 app-index.html页面所在文件夹
    # root /var/www/html;
    # index index.html;

    location / {
        proxy_pass http://localhost;
        proxy_set_header Host $host; 
        proxy_set_header X-Real-IP $remote_addr; 
        proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for; 
        # try_files $uri $uri/ /index.html;
    }
}
```

## docker 文件

```
FROM nginx
COPY ./nginx/customproxy.conf /etc/nginx/conf.d/ 
RUN rm /etc/nginx/conf.d/default.conf
EXPOSE 80
CMD ["nginx","-g","daemon off;"] 
```

## 生成images
```
docker build -t nginxproxy .
```

## 启动容器：
``` 
docker run -it --name nginxproxy1 --restart=always -p 8088:80 -d nginxproxy
```
