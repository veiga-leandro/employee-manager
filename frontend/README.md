# Employee Manager - Frontend

## 📋 Description
Web application for employee management, allowing users to register, edit, view, and remove employees. The system offers authentication, profile management, and organizational hierarchy functionalities.

## 🚀 Technologies Used
- **React**: Library for building the user interface
- **TypeScript**: Strongly typed programming language
- **Material UI**: Component framework for React
- **Formik**: Form management library
- **Yup**: Form validation library
- **Axios**: HTTP client for API communication
- **React Router**: Application routing

## ⚙️ Requirements
- **Node.js** (v14 or higher)
- **npm** or **yarn**

## 🔧 Installation

Clone the repository:

```sh
git clone https://github.com/veiga-leandro/employee-manager.git
```

Navigate to the solution directory:

```sh
cd frontend
```

Install dependencies:
```bash
npm install
# or
yarn install
```

Configure the environment:
Create a `.env` file in the project root with the following variables:
```env
REACT_APP_API_URL=https://localhost:44359/api
```

## 🏃‍♂️ How to Run
```bash
npm start
# or
yarn start
```
The application will be available at: [http://localhost:3000](http://localhost:3000)

## 🏛️ Project Structure
```
src/
├── api/          # Services and Axios configuration
├── assets/       # Static files (images, icons)
├── components/   # Reusable components
│   ├── common/   # Generic components (buttons, inputs, etc.)
│   └── layout/   # Layout components (header, footer, etc.)
├── contexts/     # React contexts (authentication, etc.)
├── models/       # Interfaces and types
├── pages/        # Application pages/routes
├── App.tsx       # Main component
├── index.tsx     # Application entry point
```

## ✨ Key Features
- **Authentication**: Login, logout, and session control
- **Employee Management**: Full CRUD for employees
- **Hierarchy**: Visualization and management of organizational hierarchy
- **Access Profiles**: Different access levels (Admin, HR, Manager, Employee)
- **Validated Forms**: Real-time validation of input data

## 🔐 Authentication
The system uses **JWT (JSON Web Token) authentication**. Tokens are stored in **localStorage** and sent with each request via the authorization header.

## 📱 Responsiveness
The interface is fully responsive, adapting to different screen sizes using **Material UI** features.

## 📦 Build and Deployment
To generate the production version:
```bash
npm run build
# or
yarn build
```
The generated files will be in the `build/` folder.

## 🖥️ Backend Integration
This frontend application communicates with a **RESTful API developed in .NET Core**. Ensure the backend is running and properly configured.

**Main Endpoints:**
- `/api/auth` - Authentication
- `/api/employees` - Employee CRUD

## Contributing

Contributions are welcome! Please open an issue or submit a pull request.

## License

This project is licensed under the MIT License.

