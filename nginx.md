# nginx+docker同一服务器上部署多个docker实现负载均衡

## nginx配置文件

```
upstream *mynginx* {
      server 192.168.8.110:8080 weight=1;
      server 192.168.8.110:8081 weight=2;
}
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
        proxy_pass http://*mynginx*/nginx/test;
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
