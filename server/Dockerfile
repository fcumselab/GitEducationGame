# 使用官方 Node.js 基础镜像
FROM node:14

# 创建并设置应用目录
WORKDIR /usr/src/app

# 复制 package.json 和 package-lock.json 文件
COPY package*.json ./

# 安装项目依赖
RUN npm install

# 复制项目文件到容器中
COPY . .

# 暴露端口
EXPOSE 3000

# 运行应用
CMD ["npm", "start"]
