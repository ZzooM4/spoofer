<!DOCTYPE html>
<html>
<head>
	<meta charset="UTF-8">
	<meta name="viewport" content="width=device-width, initial-scale=1.0">
</head>
<body>
	<h1>Spoofer</h1>
	<p>Spoofer is a powerful software tool that can spoof various elements of your computer system to bypass game security measures. It currently supports popular games such as Valorant, Escape From Tarkov*.</p>
<h2>Features</h2>
<ul>
	<li>BIOS spoofing via AMIDEWIN</li>
	<li>Registry entry spoofing**</li>
	<li>VolumeID spoofing (RAID 0 highly recommended; VolumeID has limited effect)</li>
	<li>MAC address spoofing***</li>
	<li>Peripheral access blocking</li>
</ul>

<h2>Download</h2>
<p>Please download the latest version of the spoofer from the releases section of this repository.</p>

<h2>Prerequisites</h2>
<p>Before using the spoofer, you must first identify the appropriate version of AMIDEWIN for your system. Follow these steps:</p>
<ol>
	<li>Download and attempt the suggested version of AMIDEWIN.</li>
	<li>Launch the Command Prompt as an administrator and input the command “AMIDEWINx64.exe /IVN”.</li>
	<li>If it returns a value, the version is correct, and you can proceed to step 3. If it returns an error, follow step 2 to find the right version.</li>
	<li>Find the appropriate version of AMIDEWIN for a Lenovo ThinkCentre model with the same CPU as yours:
		<ul>
			<li>Use Google to search for a ThinkCentre model with an identical CPU to yours.</li>
			<li>Access the support page for the identified model.</li>
			<li>In the Drivers &amp; Software section, opt for the BIOS category and select “BIOS update from operating system”.</li>
			<li>Swap the AMIDEWIN files (.exe and .sys) in my folder with the newly obtained ones.</li>
		</ul>
	</li>
	<li>Re-flash your BIOS with a different version:
		<ul>
			<li>Utilize Google to locate your main board's support page and identify an alternative BIOS version.</li>
			<li>Ensure it differs from the version currently in use.</li>
			<li>Transfer the new BIOS version to a USB drive and flash it.</li>
		</ul>
	</li>
</ol>

<h2>Future Updates</h2>
<p>Here are some of the planned updates for the spoofer:</p>
<ul>
	<li>Adding support for more registry keys to improve spoofing effectiveness.</li>
	<li>Adding GPU UUID/serial spoofing for even better spoofing.</li>
	<li>Adding proper hard drive serial spoofing to eliminate the need for RAID 0.</li>
	<li>Expanding the spoofer beyond spoofing to include additional features and functionality, like a cleaner functionality.</li>
</ul>

<h2>Notes</h2>
<p>* If you want to utilize this spoofer for Escape From Tarkov, you should also use <a href="https://www.unknowncheats.me/forum/escape-from-tarkov/494040-hwho-slightly-fun-bsg-launcher-hwid-check-bypass.html">HWho</a></p>
<p>** The spoofer spoofs registry entries from <a href="https://github.com/volatilityfoundation/artifacts/tree/master/Microsoft/Windows/Registry%20Keys">here</a>, excluding a few entries which may not be too safe to change.</p>
<p>*** The MAC address spoofer modifies registry entries only. For a lasting solution, watch <a href="https://www.youtube.com/watch?v=wgJr5F0S8f4">this video</a>.</p>
</body>
</html>
