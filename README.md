<header>
  <h1>Spoofer</h1>
  <section id="intro">
    <p>Spoofer is a powerful software tool that can spoof various elements of your computer system to bypass game security measures. It currently supports popular games such as Valorant and Escape From Tarkov*</p>
  </section>
  <nav>
    <h2>Navigator</h2>
    <ul>
      <li>
        <a href="#features">Features</a>
      </li>
      <li>
        <a href="#download">Download</a>
      </li>
      <li>
        <a href="#prerequisites">Prerequisites</a>
      </li>
      <li>
        <a href="#updates">Future Updates</a>
      </li>
      <li>
        <a href="#notes">Notes</a>
      </li>
      <li>
        <a href="#disclaimer">Legal Disclaimer</a>
      </li>
    </ul>
  </nav>
</header>
<section id="features">
  <h2>Features</h2>
  <ul>
    <li>BIOS spoofing via AMIDEWIN</li>
    <li>Registry entry spoofing**</li>
    <li>VolumeID spoofing (RAID 0 highly recommended; VolumeID has limited effect)</li>
    <li>MAC address spoofing***</li>
    <li>Peripheral access blocking</li>
  </ul>
</section>
<section id="download">
  <h2>Download</h2>
  <p>Please download the latest version of the Spoofer from the releases section of this repository.</p>
</section>
<section id="prerequisites">
  <h2>Prerequisites</h2>
  <p>Before using the Spoofer, you must first identify the appropriate version of AMIDEWIN for your system. Follow these steps:</p>
  <ol>
    <li>Download and attempt the suggested version of AMIDEWIN.</li>
    <li>Launch the Command Prompt as an administrator and input the command &ldquo;AMIDEWINx64.exe /IVN&rdquo;.</li>
    <li>If it returns a value, the version is correct, and you can proceed to step 3. If it returns an error, follow step 2 to find the right version.</li>
    <li>Find the appropriate version of AMIDEWIN for a Lenovo ThinkCentre model with the same CPU as yours: <ul>
        <li>Use Google to search for a ThinkCentre model with an identical CPU to yours.</li>
        <li>Access the support page for the identified model.</li>
        <li>In the Drivers &amp; Software section, opt for the BIOS category and select &ldquo;BIOS update from operating system&rdquo;.</li>
        <li>Swap the AMIDEWIN files (.exe and .sys) in my folder with the newly obtained ones.</li>
      </ul>
    </li>
    <li>Re-flash your BIOS with a different version: <ul>
        <li>Utilize Google to locate your main board's support page and identify an alternative BIOS version.</li>
        <li>Ensure it differs from the version currently in use.</li>
        <li>Transfer the new BIOS version to a USB drive and flash it.</li>
      </ul>
    </li>
  </ol>
</section>
<section id="updates">
  <h2>Future Updates</h2>
  <p>Here are some of the planned updates for the Spoofer:</p>
  <ul>
    <li>Adding support for more registry keys to improve spoofing effectiveness.</li>
    <li>Adding GPU UUID/serial spoofing for even better spoofing.</li>
    <li>Adding proper hard drive serial spoofing to eliminate the need for RAID 0.</li>
    <li>Expanding the Spoofer beyond spoofing to include additional features and functionality, like a cleaner functionality.</li>
  </ul>
</section>
<section id="notes">
  <h2>Notes</h2>
  <ul>
    <li>*If you want to use this Spoofer for Escape From Tarkov, you should also use <a href="https://www.unknowncheats.me/forum/escape-from-tarkov/494040-hwho-slightly-fun-bsg-launcher-hwid-check-bypass.html">HWho</a>. </li>
    <li>**The Spoofer spoofs registry entries from <a href="https://github.com/volatilityfoundation/artifacts/tree/master/Microsoft/Windows/Registry%20Keys">here</a>, excluding a few entries that may not be safe to change. </li>
    <li>***The MAC address spoofer modifies registry entries only. For a lasting solution, watch <a href="https://www.youtube.com/watch?v=wgJr5F0S8f4">this video</a>. </li>
  </ul>
</section>
<section id="disclaimer">
  <h2>Legal Disclaimer</h2>
	<p>The Spoofer software provided on this repository is intended for educational purposes only. The owner of the repository is not liable for any illegal or unauthorized use of the software. It is the responsibility of the user to comply with all applicable laws and regulations.</p>
	<p>The software is provided "as is" without warranty of any kind, either express or implied, including but not limited to the implied warranties of merchantability and fitness for a particular purpose. The owner of the repository does not guarantee the accuracy or completeness of the software or any information provided in connection with it. The user assumes all risks associated with the use of the software.</p>
	<p>By downloading or using the software, the user agrees to these terms and conditions.</p>
	<p>Any actions and/or activities performed using the Spoofer software are solely your responsibility. The misuse of the Spoofer software can result in criminal charges brought against the persons in question. The author will not be held responsible in the event any criminal charges be brought against any individuals misusing the Spoofer software to break the law.</p>
	<p>The Spoofer software should not be used to do harm and is only intended for educational and ethical purposes. If you are unsure about the legality of using the Spoofer software, please consult legal counsel before proceeding.</p>
</section>
