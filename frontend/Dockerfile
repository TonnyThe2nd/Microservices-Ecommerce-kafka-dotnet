FROM node:20-slim
WORKDIR /app          
RUN npm install -g @angular/cli
COPY package*.json ./
RUN npm install
RUN npm install @rollup/rollup-linux-x64-gnu --save-optional
COPY . .
CMD ["ng", "serve", "--host", "0.0.0.0", "--poll=2000"]