# nginx+docker同一服务器上部署多个docker实现负载均衡

## nginx配置文件

## 配置主机host文件 ip 192.168.8.110 对应 www.abc.com 
 
```
upstream www.abc.com {
      server 192.168.8.110:8080 weight=1;
      server 192.168.8.110:8081 weight=2;
}
server {
    listen 80;
    listen [::]:80;
    # 接口服务的IP地址
    server_name www.abc.com;
    charset utf-8;
    access_log off;
    # ElecManageSystem-应用文件夹名称 app-index.html页面所在文件夹
    root /var/www/html;
    index index.html;

    location / {
        proxy_pass http://www.abc.com;
        proxy_set_header Host $host; 
        proxy_set_header X-Real-IP $remote_addr; 
        proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for; 
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
