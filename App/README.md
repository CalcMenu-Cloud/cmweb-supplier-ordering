# OrderingApp

An Angular-based ordering interface application for managing orders, user sessions, and client interactions.

## Overview

OrderingApp is a web application built with Angular 14 that provides a complete ordering management system. It allows users to browse orders, view order details, manage user sessions, and interact with a backend API for order processing.

## Features

- **User Authentication**: Login system with session management
- **Order Management**: View, list, and manage orders
- **Session Security**: Token-based authentication with refresh tokens
- **Responsive Design**: Works on desktop and mobile devices
- **Modal Dialogs**: Confirmation and message dialogs
- **API Integration**: Communicates with backend services

## Technology Stack

| Category | Technology |
|----------|------------|
| Framework | Angular 14 |
| UI Components | Angular Material 14 |
| Styling | Bootstrap 5, SCSS |
| Alerts | SweetAlert2 |
| Reactive | RxJS 7.5 |
| Testing | Karma, Jasmine |

---

## Framework Requirements

### Core Framework
| Requirement | Version | Description |
|-------------|---------|-------------|
| Angular | ^14.0.0 | Core framework |
| Angular CLI | ~14.0.2 | Command-line interface |
| Angular Common | ^14.0.0 | Common Angular modules |
| Angular Compiler | ^14.0.0 | Angular template compiler |
| Angular Core | ^14.0.0 | Core Angular runtime |
| Angular Forms | ^14.0.0 | Forms module |
| Angular Platform Browser | ^14.0.0 | Browser platform |
| Angular Platform Browser Dynamic | ^14.0.0 | Dynamic platform |
| Angular Router | ^14.0.0 | Routing module |

### UI Libraries
| Requirement | Version | Description |
|-------------|---------|-------------|
| Angular Material | ^14.0.3 | Material Design components |
| Angular CDK | ^14.0.3 | Component Dev Kit |
| Bootstrap | ^5.2.0 | CSS framework |
| SweetAlert2 | ^11.4.26 | Alert dialogs |

### Development Dependencies
| Requirement | Version | Description |
|-------------|---------|-------------|
| TypeScript | ~4.7.2 | Language |
| @angular-devkit/build-angular | ^14.0.2 | Build system |
| @angular/compiler-cli | ^14.0.0 | Compiler CLI |
| Karma | ~6.3.0 | Test runner |
| Karma Chrome Launcher | ~3.1.0 | Chrome browser |
| Karma Jasmine | ~5.0.0 | Jasmine adapter |
| Karma Jasmine HTML Reporter | ~1.7.0 | HTML reporter |
| Jasmine Core | ~4.1.0 | Testing framework |
| @types/jasmine | ~4.0.0 | TypeScript types |

### Runtime Dependencies
| Requirement | Version | Description |
|-------------|---------|-------------|
| RxJS | ~7.5.0 | Reactive programming |
| tslib | ^2.3.0 | TypeScript library |
| zone.js | ~0.11.4 | Angular zone |

---

## Additional Requirements

### Node.js
- **Minimum Version**: Node.js 14.x or higher
- **Recommended Version**: Node.js 16.x LTS or 18.x LTS
- Check installed version: `node --version`

### npm
- **Minimum Version**: npm 6.x or higher
- **Recommended Version**: npm 8.x or higher
- Check installed version: `npm --version`

### Browser Support
- Chrome (latest)
- Firefox (latest)
- Edge (latest)
- Safari (latest)

### Build Tools
- Windows PowerShell 5.1+ or Command Prompt
- Git (optional, for version control)

---

## Project Structure

```
order_app/
├── src/
│   ├── app/
│   │   ├── header/           # Header component
│   │   ├── footer/           # Footer component
│   │   ├── pages/
│   │   │   ├── login/        # Login page
│   │   │   ├── loginsuccess/ # Login success page
│   │   │   ├── orderlist/    # Order list page
│   │   │   └── orderview/    # Order view page
│   │   ├── services/         # API services
│   │   ├── interfaces/      # TypeScript interfaces
│   │   └── modal/            # Modal components
│   ├── environments/         # Environment configs
│   └── styles.scss           # Global styles
├── scripts/                  # SQL and data scripts
├── angular.json              # Angular CLI config
├── package.json              # Dependencies
└── tsconfig.json             # TypeScript config
```

---

## Installation Steps

### 1. Install Node.js
Download from: https://nodejs.org/

### 2. Install Dependencies
```powershell
cd c:\Data\PROJECT\OrderingInterface\OrderingApp\order_app
npm install
```

### 3. Run Development Server
```powershell
npm start
# or
ng serve
```
Access at: http://localhost:4200

### 4. Build for Production
```powershell
npm run build
# or
ng build
```

### 5. Run Tests
```powershell
npm test
# or
ng test
```

---

## How to Run the Application

### Quick Start
```powershell
# Navigate to project directory
cd c:\Data\PROJECT\OrderingInterface\OrderingApp\order_app

# Install dependencies (first time only)
npm install

# Start the development server
npm start
```

### Detailed Run Options

#### Option 1: Using npm scripts
```powershell
# Standard start (default port 4200)
npm start

# With custom port
npm start -- --port 4201

# Enable hot reload
npm start -- --open
```

#### Option 2: Using Angular CLI directly
```powershell
# Basic serve
ng serve

# With custom host and port
ng serve --host 0.0.0.0 --port 4200

# Open browser automatically
ng serve --open

# Enable source maps for debugging
ng serve --source-map
```

#### Option 3: Production Build & Serve
```powershell
# Build for production
npm run build

# Serve production build (requires a web server)
# Option A: Using http-server
npx http-server dist/egs_pimupdate-app

# Option B: Using Angular CLI serve with production config
ng serve --configuration production
```

### Development Server Features
- **Hot Module Replacement (HMR)**: Changes automatically reload without full refresh
- **Live Reload**: Browser refreshes on file changes
- **Source Maps**: Debug TypeScript in browser dev tools
- **Error Display**: Compilation errors shown in terminal and browser

### Accessing the Application

| Environment | URL | Description |
|-------------|-----|-------------|
| Development | http://localhost:4200 | Local dev server |
| Production | (depends on deployment) | Built application |

### Troubleshooting

#### Port Already in Use
```powershell
# Find process using port 4200
netstat -ano | findstr :4200

# Kill the process (replace PID with actual process ID)
taskkill /PID <PID> /F

# Or use a different port
ng serve --port 4201
```

#### Node Version Issues
```powershell
# Check Node version
node --version

# Check npm version
npm --version

# If version is too old, update Node.js from https://nodejs.org/
```

#### Clear Cache and Rebuild
```powershell
# Remove node_modules and reinstall
rmdir /s /node_modules
npm install

# Or just clear Angular cache
rmdir /s /dist
rmdir /s /.angular
npm start
```

#### Memory Issues (Large Projects)
```powershell
# Increase Node memory limit
node --max_old_space_size=4096 ./node_modules/@angular/cli/bin/ng serve

# Or use the npm script
npm run build-serve
```

---

## How to Deploy the Application

### Pre-deployment Checklist
- [ ] Run `npm test` to ensure all tests pass
- [ ] Run `ng build --configuration production` to verify production build
- [ ] Check for any console errors
- [ ] Verify environment configuration in `environment.prod.ts`
- [ ] Ensure all API endpoints are accessible

### Build for Production

#### Step 1: Create Production Build
```powershell
# Navigate to project directory
cd c:\Data\PROJECT\OrderingInterface\OrderingApp\order_app

# Clean previous build
rmdir /s /q dist

# Build production version
npm run build
# or
ng build --configuration production
```

#### Step 2: Output Location
- Build output: `dist/egs_pimupdate-app/`
- Contains optimized and minified files

---

### Deployment Options

#### Option 1: Static Web Server (IIS)
1. **Publish the build folder**
   ```
   Copy dist\egs_pimupdate-app\* to IIS website root
   ```

2. **Configure web.config**
   - Ensure URL rewrite module is installed
   - Add proper MIME types for Angular assets

3. **Sample web.config for Angular**
   ```xml
   <?xml version="1.0" encoding="utf-8"?>
   <configuration>
     <system.webServer>
       <rewrite>
         <rules>
           <rule name="Angular Routes" stopProcessing="true">
             <match url=".*" />
             <conditions logicalGrouping="MatchAll">
               <add input="{REQUEST_FILENAME}" matchType="IsFile" negate="true" />
               <add input="{REQUEST_FILENAME}" matchType="IsDirectory" negate="true" />
             </conditions>
             <action type="Rewrite" url="/index.html" />
           </rule>
         </rules>
       </rewrite>
     </system.webServer>
   </configuration>
   ```

#### Option 2: Static Web Server (Apache)
1. **Publish the build folder**
   ```
   Copy dist\egs_pimupdate-app\* to Apache document root
   ```

2. **Configure .htaccess for Angular routing**
   ```apache
   RewriteEngine On
   RewriteBase /
   RewriteRule ^index\.html$ - [L]
   RewriteCond %{REQUEST_FILENAME} !-f
   RewriteCond %{REQUEST_FILENAME} !-d
   RewriteRule . /index.html [L]
   ```

#### Option 3: Azure Static Web Apps
```powershell
# Install Azure Static Web Apps CLI
npm install -g @azure/static-web-apps-cli

# Build the app
ng build --configuration production

# Deploy
az staticwebapp compose \
  --name <your-app-name> \
  --resource-group <your-resource-group> \
  --source dist/egs_pimupdate-app
```

#### Option 4: AWS S3 + CloudFront
1. **Upload to S3**
   ```powershell
   aws s3 sync dist/egs_pimupdate-app s3://your-bucket-name --delete
   ```

2. **Configure CloudFront** for caching
3. **Set up Route 53** for DNS

#### Option 5: Docker Container
1. **Create Dockerfile**
   ```dockerfile
   # filepath: Dockerfile
   FROM node:18-alpine as build
   WORKDIR /app
   COPY package*.json ./
   RUN npm ci
   COPY . .
   RUN npm run build

   FROM nginx:alpine
   COPY --from=build /app/dist/egs_pimupdate-app /usr/share/nginx/html
   COPY nginx.conf /etc/nginx/conf.d/default.conf
   EXPOSE 80
   CMD ["nginx", "-g", "daemon off;"]
   ```

2. **Create nginx.conf**
   ```nginx
   server {
       listen 80;
       server_name localhost;
       root /usr/share/nginx/html;
       index index.html;

       location / {
           try_files $uri $uri/ /index.html;
       }
   }
   ```

3. **Build and Run**
   ```powershell
   docker build -t ordering-app .
   docker run -p 80:80 ordering-app
   ```

#### Option 6: GitHub Pages
```powershell
# Install angular-cli-ghpages
npm install -g angular-cli-ghpages

# Build and deploy
ng build --configuration production --base-href "https://your-org.github.io/repo/"
npx angular-cli-ghpages --repo "https://github.com/your-org/repo" --branch gh-pages
```

---

### Environment Configuration

#### Production Environment Variables
Edit `src/environments/environment.prod.ts`:

```typescript
export const environment = {
  production: true,
  apiUrl: 'https://your-production-api.com',
  // Add other environment-specific variables
};
```

---

### Post-deployment Verification

| Check | Description |
|-------|-------------|
| Application loads | Home page renders correctly |
| Navigation works | All routes are accessible |
| API calls succeed | Backend communication works |
| No console errors | Check browser developer tools |
| Responsive design | Works on mobile/tablet |
| Performance | Page load under 3 seconds |

---

### CI/CD Deployment (Optional)

#### GitHub Actions Example
```yaml
# filepath: .github/workflows/deploy.yml
name: Deploy to Production

on:
  push:
    branches: [main]

jobs:
  build-and-deploy:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v3
      
      - name: Setup Node.js
        uses: actions/setup-node@v3
        with:
          node-version: '18'
          
      - name: Install dependencies
        run: npm ci
        
      - name: Build
        run: npm run build --if-present
        
      - name: Deploy
        run: |
          # Add your deployment commands here
```

#### Azure DevOps Example
```yaml
# Azure Pipeline for Angular deployment
trigger:
- main

pool:
  vmImage: 'ubuntu-latest'

steps:
- task: NodeTool@0
  inputs:
    versionSpec: '18.x'
  displayName: 'Install Node.js'

- script: |
    npm ci
    npm run build --if-present
  displayName: 'Build Angular App'

- task: PublishPipelineArtifact@1
  inputs:
    targetPath: '$(System.DefaultWorkingDirectory)/dist/egs_pimupdate-app'
    artifact: 'dist'
```

---

## License

This project is proprietary and confidential.
