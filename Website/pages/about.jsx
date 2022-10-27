// Next js imports
import Head from "next/head";
import Link from "next/link";
import Image from "next/image";
import Script from "next/script";

// CSS imports
import styles from "../styles/pages/About.module.css";

// Component imports

// Page
export default function About() {
  return (
    <>
      {/* Meta tags */}
      <Head>
        <title>test</title>
      </Head>

      {/* Main web content excluding navbar and footer */}
      <section>
        <Link href="/">About content</Link>
        <Image src="/img/a.gif" alt="L" width="100px" height="150px" />
      </section>
    </>
  );
}
