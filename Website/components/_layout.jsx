// Imports
import Head from "next/head";
import Link from "next/link";
import Image from "next/image";
import Script from "next/script";

// CSS imports

// Component imports
import Navbar from "./parts/Navbar";
import Footer from "./parts/Footer";
import Preloader from "./parts/Preloader";
import { Suspense } from "react";

// Page
export default function Layout({ children }) {
  return (
    <>
      <Head>
        <link rel="icon" href="/favicon.ico" />
      </Head>

      {/* Webpage global layout */}
      <Suspense fallback={<Preloader text="Loading page..." />}>
        <Script
          src="https://kit.fontawesome.com/8ee8b8ceb7.js"
          crossorigin="anonymous"
        ></Script>
        <Script src="/scripts/LoadEffect.js" />

        <Navbar />
        {children}
        <Footer />
      </Suspense>
    </>
  );
}
