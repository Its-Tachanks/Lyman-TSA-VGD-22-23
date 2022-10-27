// Imports
import Head from "next/head";
import Link from "next/link";
import Image from "next/image";
import Script from "next/script";

import { useRef, useEffect } from "react";

// CSS imports
import styles from "../../styles/parts/Navbar.module.css";

// Component imports
import { Tooltip, IconButton, Icon } from "@mui/material";

// Page
export default function Navbar(props) {
  const navbarRef = useRef();

  useEffect(() => {
    window.addEventListener("scroll", () => {
      //console.log(document.body.getBoundingClientRect().top);
      document.body.getBoundingClientRect().top <= -1
        ? navbarRef.current.classList.add(`${styles.blackBg}`)
        : navbarRef.current.classList.remove(`${styles.blackBg}`);
    });
  }, []);

  return (
    <nav
      className="d-flex flex-row justify-content-around align-items-center"
      id={`${styles.navbar}`}
      ref={navbarRef}
    >
      <div
        className={`container-fluid d-flex justify-content-center align-items-center ${styles.navbarContainer}`}
      >
        <Tooltip title="test" leaveDelay={100}>
          <IconButton sx={{ color: "var(--icon-color)" }}>
            <i className="fa-solid fa-bolt"></i>
          </IconButton>
        </Tooltip>
      </div>
      <div
        className={`container-fluid d-flex justify-content-center align-items-center ${styles.navbarContainer}`}
      >
        <Tooltip title="test" leaveDelay={100}>
          <IconButton sx={{ color: "var(--icon-color)" }}>
            <i className="fa-sharp fa-solid fa-mug-hot"></i>
          </IconButton>
        </Tooltip>
      </div>
      <div
        className={`container-fluid d-flex justify-content-center align-items-center ${styles.navbarContainer}`}
      >
        <Tooltip title="test" leaveDelay={100}>
          <IconButton sx={{ color: "var(--icon-color)" }}>
            <i className="fa-brands fa-docker"></i>
          </IconButton>
        </Tooltip>
      </div>
    </nav>
  );
}
