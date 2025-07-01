

import { initializeApp } from "firebase/app"
import { getAuth, GoogleAuthProvider, signInWithPopup, signOut } from "firebase/auth"

const firebaseConfig = {
  apiKey: "AIzaSyCvr8JGgbYA1jW8tC3_IFHpURmJpRRYZn8",
  authDomain: "todo-f89e2.firebaseapp.com",
  projectId: "todo-f89e2",
  storageBucket: "todo-f89e2.firebasestorage.app",
  messagingSenderId: "594875742432",
  appId: "1:594875742432:web:7d2b25301e073214a33885",
  measurementId: "G-ER9D5R6YWE"
};

const app = initializeApp(firebaseConfig)
export const auth = getAuth(app)
export const provider = new GoogleAuthProvider()
export { signInWithPopup, signOut }